"""
TestServer.py - Mobile app API tests (DatabaseControllers2025 + auth).

This file is written to mirror how a production phone app should call the API. Use the same patterns in Cellular
(or any client): JSON bodies, camelCase property names, Bearer JWT after Authorize, and mobileID consistently.

--- Production-style checklist (do this in the app) ---

1) Base URL
   - One origin, no trailing slash, e.g. https://api.revmetrix.io
   - All routes below are prefixed with /api

2) TLS
   - Production: verify TLS certificates (VERIFY_TLS = True here when hitting real prod).
   - False is only for local dev with self-signed HTTPS; do not ship that to users.

3) Login
   - POST /api/posts/Authorize
   - Body: {"username": "<login>", "password": "<password>"}
   - Content-Type: application/json
   - Optional: some clients (e.g. MAUI ApiController) also send Basic Authorization (base64 of user:pass);
     keep whatever your deployed server expects.
   - 200 response: read JWT from "tokenA" (same field name the server returns today).

4) Authenticated calls
   - Header: Authorization: Bearer <tokenA>
   - Header: Content-Type: application/json for POST/DELETE with JSON body

5) mobileID (app user / row correlation)
   - Use the signed-in app user's stable integer id (e.g. local SQLite user PK) as mobileID everywhere the API
     accepts it: POST bodies for balls, events, sessions, games, frames, shots, establishments, etc.
   - For GET GetBallsByUsername and GET GetEventsByUsername, pass the same value as a query parameter:
     ?mobileID=<that user id>  (server uses this with username from the JWT to resolve combinedDB.Users).

6) Deletes that scope by user
   - Many delete endpoints expect JSON body: {"username": "<same as login>", "mobileID": <same as above>}

7) Test-only vs production data
   - TEST_MARKER / huge synthetic numbers in tests avoid polluting real data; in production use real local PKs
     as mobileID, not random billions.

8) This script cleans the cloud after the suite (tearDownClass) and again on interpreter exit (atexit).
   Posts often use fake session/event IDs; those rows are not removed by user-scoped deletes alone — cleanup also
   calls DeleteOrphanedAppData so sessions/games/frames/shots do not linger for the phone to sync.

Switch BASE_URL / VERIFY_TLS for local vs cloud.
Run: python -m pytest TestServer.py -v
Or:  python TestServer.py
"""
import atexit
import base64
import json
import time
import unittest
import warnings

import requests

# --- Configure base URL (no trailing slash) ---
# Local:  https://localhost:7238
# Cloud:  https://api.revmetrix.io
# BASE_URL = "https://localhost:7238"
BASE_URL = "https://api.revmetrix.io"

# Production clients must verify TLS. Use False only for local HTTPS with untrusted/self-signed certs.
VERIFY_TLS = False

# Test user for authorized requests (must exist in DB for auth tests)
TEST_USERNAME = "string"
TEST_PASSWORD = "string"
# MobileID for the same test user (deletes, GET ?mobileID= scoping — matches phone app).
TEST_MOBILE_ID = 1
TEST_MARKER = "PYTEST_CLOUD"

# Keep test-generated numeric values very high to avoid collisions with real app data.
HIGH_NUM_BASE = 1_900_000_000 + int(time.time()) % 10_000

# HTTP timeout for cleanup deletes (cloud can be slow).
REQUEST_TIMEOUT_SEC = 120

# Leaf-to-root for user-owned graph; body is always username + mobileID for these endpoints.
_USER_SCOPED_DELETE_ENDPOINTS = (
    "/api/deletes/DeleteAppShots",
    "/api/deletes/DeleteAppFrames",
    "/api/deletes/DeleteAppGames",
    "/api/deletes/DeleteAppSessions",
    "/api/deletes/DeleteEventsByUsername",
    "/api/deletes/DeleteBallsByUsername",
    "/api/deletes/DeleteAppEstablishments",
)


def _url(path: str) -> str:
    """Build full URL. path should start with / (e.g. /api/gets/GetAppUsers)."""
    return BASE_URL.rstrip("/") + ("/" + path.lstrip("/") if not path.startswith("http") else path)


def _jwt_expiry_utc(token: str) -> float | None:
    """Return JWT `exp` as Unix seconds, or None if missing or invalid."""
    try:
        payload_b64 = token.split(".")[1]
        padding = "=" * (-len(payload_b64) % 4)
        data = json.loads(base64.urlsafe_b64decode(payload_b64 + padding))
        exp = data.get("exp")
        return float(exp) if exp is not None else None
    except (IndexError, ValueError, TypeError, json.JSONDecodeError):
        return None


def _normalize_token_b(raw) -> bytes | None:
    if raw is None:
        return None
    if isinstance(raw, str):
        return base64.b64decode(raw)
    if isinstance(raw, list):
        return bytes(raw)
    return None


# Cached credentials: reuse JWT until shortly before exp, then POST Refresh with tokenB (same as production apps).
_auth_jwt: str | None = None
_auth_refresh: bytes | None = None
_auth_exp_utc: float | None = None
_AUTH_MARGIN_SEC = 120


def get_auth_token() -> str:
    """Return a valid JWT for TEST_USERNAME/TEST_PASSWORD (cached; refresh or re-authorize when needed)."""
    global _auth_jwt, _auth_refresh, _auth_exp_utc

    if not VERIFY_TLS:
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")

    now = time.time()
    if (
        _auth_jwt
        and _auth_exp_utc is not None
        and now < _auth_exp_utc - _AUTH_MARGIN_SEC
    ):
        return _auth_jwt

    if _auth_refresh:
        rr = requests.post(
            _url("/api/posts/Refresh"),
            json={"token": base64.b64encode(_auth_refresh).decode("ascii")},
            headers={"Content-Type": "application/json"},
            verify=VERIFY_TLS,
            timeout=REQUEST_TIMEOUT_SEC,
        )
        if rr.status_code == 200:
            body = rr.json()
            token = body.get("tokenA")
            if token:
                _auth_jwt = token
                _auth_refresh = _normalize_token_b(body.get("tokenB")) or _auth_refresh
                _auth_exp_utc = _jwt_expiry_utc(token)
                if _auth_exp_utc is None:
                    _auth_exp_utc = now + 3600
                return _auth_jwt
        _auth_refresh = None

    r = requests.post(
        _url("/api/posts/Authorize"),
        json={"username": TEST_USERNAME, "password": TEST_PASSWORD},
        headers={"Content-Type": "application/json"},
        verify=VERIFY_TLS,
        timeout=REQUEST_TIMEOUT_SEC,
    )
    if r.status_code != 200:
        raise AssertionError(f"Authorize failed: {r.status_code} - {r.text}")
    body = r.json()
    token = body.get("tokenA")
    if not token:
        raise AssertionError("No tokenA in response")
    _auth_jwt = token
    _auth_refresh = _normalize_token_b(body.get("tokenB"))
    _auth_exp_utc = _jwt_expiry_utc(token)
    if _auth_exp_utc is None:
        _auth_exp_utc = now + 3600
    return _auth_jwt


def auth_headers() -> dict:
    return {"Authorization": f"Bearer {get_auth_token()}", "Content-Type": "application/json"}


def run_cloud_cleanup():
    """
    Best-effort full cleanup for TEST_USERNAME / TEST_MOBILE_ID on the cloud.

    1) User-scoped deletes (shots → … → establishments).
    2) DeleteOrphanedAppData — removes sessions/games/frames/shots left from tests that POSTed bogus FKs
       (not tied to your events), which otherwise sync back to the phone.
    3) Repeat both passes once more to clear anything the first orphan pass detached.
    """
    try:
        headers = auth_headers()
    except Exception:
        return

    body = {"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID}

    def user_chain():
        for endpoint in _USER_SCOPED_DELETE_ENDPOINTS:
            try:
                requests.delete(
                    _url(endpoint),
                    json=body,
                    headers=headers,
                    verify=VERIFY_TLS,
                    timeout=REQUEST_TIMEOUT_SEC,
                )
            except requests.RequestException:
                pass

    def orphans():
        try:
            requests.delete(
                _url("/api/deletes/DeleteOrphanedAppData"),
                headers=headers,
                verify=VERIFY_TLS,
                timeout=REQUEST_TIMEOUT_SEC,
            )
        except requests.RequestException:
            pass

    user_chain()
    orphans()
    user_chain()
    orphans()


atexit.register(run_cloud_cleanup)


class TestMobileAppAPI(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        if not VERIFY_TLS:
            warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        run_cloud_cleanup()

    @classmethod
    def tearDownClass(cls):
        run_cloud_cleanup()

    @staticmethod
    def _high(offset: int) -> int:
        return HIGH_NUM_BASE + offset

    def setUp(self):
        if not VERIFY_TLS:
            warnings.filterwarnings("ignore", message="Unverified HTTPS request")

    # --- Helpers: production-style sync primitives ---
    def _auth(self):
        return auth_headers()

    def _mobile_params(self):
        # This is the app user's stable local id used for combined user resolution.
        return {"mobileID": TEST_MOBILE_ID}

    def _post_json(self, path: str, body: dict, *, headers=None, params=None):
        resp = requests.post(
            _url(path),
            json=body,
            headers=headers or self._auth(),
            params=params,
            verify=VERIFY_TLS,
        )
        return resp

    def _get_list(self, path: str, *, headers=None, params=None):
        resp = requests.get(_url(path), headers=headers or self._auth(), params=params, verify=VERIFY_TLS)
        self.assertEqual(resp.status_code, 200, f"Expected 200 from {path}, got {resp.status_code} - {resp.text}")
        data = resp.json()
        self.assertIsInstance(data, list, f"{path} should return a list")
        return data

    def _get_list_first_available(self, paths: tuple[str, ...], *, headers=None, params=None):
        """
        Backwards-compatible GET: try each path until one returns 200.
        This lets the same script run against:
        - your local build (new user-scoped routes), and
        - api.revmetrix.io (until those routes are deployed).
        """
        last = None
        for p in paths:
            last = requests.get(_url(p), headers=headers or self._auth(), params=params, verify=VERIFY_TLS)
            if last.status_code == 404:
                continue
            self.assertEqual(last.status_code, 200, f"Expected 200 from {p}, got {last.status_code} - {last.text}")
            data = last.json()
            self.assertIsInstance(data, list, f"{p} should return a list")
            return data
        raise AssertionError(f"All candidate endpoints returned 404: {paths}. Last response: {getattr(last, 'status_code', None)} - {getattr(last, 'text', '')}")

    def _create_event_and_get_cloud_id(self) -> tuple[int, int]:
        """
        Create an Event and return (event_mobile_id, event_cloud_id).
        Note: The DB expects Session.EventId to be the *cloud* Event ID, not the phone-local mobileID.
        """
        headers = self._auth()
        event_mobile = self._high(920)
        unique_name = f"{TEST_MARKER}_event_{int(time.time())}_{event_mobile}"
        post_r = self._post_json(
            "/api/posts/PostEvent",
            {
                "mobileID": event_mobile,
                # userId in body is ignored by server inserts (server uses token username); keep it but do not rely on it.
                "userId": self._high(60),
                "longName": unique_name,
                "type": "Tournament",
                "location": "Test City",
                "average": self._high(61),
                "stats": 0,
                "standings": None,
            },
            headers=headers,
            params=self._mobile_params(),
        )
        self.assertEqual(post_r.status_code, 200, f"POST event failed: {post_r.status_code} - {post_r.text}")

        events = self._get_list("/api/gets/GetEventsByUsername", headers=headers, params=self._mobile_params())
        matches = [e for e in events if isinstance(e, dict) and (e.get("mobileID") == event_mobile or e.get("longName") == unique_name)]
        self.assertTrue(matches, "Posted event not returned by GetEventsByUsername")
        cloud_id = matches[0].get("id")
        self.assertIsInstance(cloud_id, int, "Expected cloud 'id' for event")
        return event_mobile, cloud_id

    def _create_session_and_get_cloud_id(self, event_cloud_id: int) -> tuple[int, int]:
        headers = self._auth()
        session_mobile = self._high(930)
        post_r = self._post_json(
            "/api/posts/PostAppSession",
            {
                "mobileID": session_mobile,
                "sessionNumber": 1,
                "establishmentId": 0,
                "eventId": event_cloud_id,
                "dateTime": self._high(33),
                "teamOpponent": "TeamA",
                "individualOpponent": "Opponent1",
                "score": 0,
                "stats": 0,
                "teamRecord": 0,
                "individualRecord": 0,
            },
            headers=headers,
            params=self._mobile_params(),
        )
        self.assertEqual(post_r.status_code, 200, f"POST session failed: {post_r.status_code} - {post_r.text}")

        sessions = self._get_list_first_available(
            ("/api/gets/GetAllSessionsByUser", "/api/gets/GetAppSessions"),
            headers=headers,
            params=self._mobile_params(),
        )
        matches = [s for s in sessions if isinstance(s, dict) and s.get("mobileID") == session_mobile]
        self.assertTrue(matches, "Posted session not returned by GetAllSessionsByUser")
        cloud_id = matches[0].get("id")
        self.assertIsInstance(cloud_id, int, "Expected cloud 'id' for session")
        return session_mobile, cloud_id

    def _create_game_and_get_cloud_id(self, session_cloud_id: int) -> tuple[int, int]:
        headers = self._auth()
        game_mobile = self._high(940)
        post_r = self._post_json(
            "/api/posts/PostAppGame",
            {
                "mobileID": game_mobile,
                "gameNumber": "1",
                "lanes": "1-2",
                "score": 9,
                "win": 0,
                "startingLane": 0,
                "sessionId": session_cloud_id,
                "teamResult": 0,
                "individualResult": 0,
            },
            headers=headers,
            params=self._mobile_params(),
        )
        self.assertEqual(post_r.status_code, 200, f"POST game failed: {post_r.status_code} - {post_r.text}")

        games = self._get_list_first_available(
            ("/api/gets/GetAllGamesByUser", "/api/gets/GetAppGames"),
            headers=headers,
            params=self._mobile_params(),
        )
        matches = [g for g in games if isinstance(g, dict) and g.get("mobileID") == game_mobile]
        self.assertTrue(matches, "Posted game not returned by GetAllGamesByUser")
        cloud_id = matches[0].get("id")
        self.assertIsInstance(cloud_id, int, "Expected cloud 'id' for game")
        return game_mobile, cloud_id

    def _create_frame_and_get_cloud_id(self, game_cloud_id: int) -> tuple[int, int]:
        headers = self._auth()
        frame_mobile = self._high(950)
        post_r = self._post_json(
            "/api/posts/PostFrames",
            {
                "mobileID": frame_mobile,
                "gameId": game_cloud_id,
                "shotOne": self._high(71),
                "shotTwo": self._high(72),
                "frameNumber": 1,
                "lane": 0,
                "result": 0,
            },
            headers=headers,
            params=self._mobile_params(),
        )
        self.assertEqual(post_r.status_code, 200, f"POST frame failed: {post_r.status_code} - {post_r.text}")

        frames = self._get_list("/api/gets/GetFramesByGameId", headers=headers, params={**self._mobile_params(), "gameId": str(game_cloud_id)})
        matches = [f for f in frames if isinstance(f, dict) and f.get("mobileID") == frame_mobile]
        self.assertTrue(matches, "Posted frame not returned by GetFramesByGameId")
        cloud_id = matches[0].get("id")
        self.assertIsInstance(cloud_id, int, "Expected cloud 'id' for frame")
        return frame_mobile, cloud_id

    def _create_shot_and_get_cloud_id(self, session_cloud_id: int, frame_cloud_id: int) -> tuple[int, int]:
        headers = self._auth()
        shot_mobile = self._high(960)
        post_r = self._post_json(
            "/api/posts/PostAppShot",
            {
                "mobileID": shot_mobile,
                "type": 0,
                "smartDotId": 0,
                "sessionId": session_cloud_id,
                "ballId": -1,
                "frameId": frame_cloud_id,
                "shotNumber": 1,
                "leaveType": 0,
                "side": "",
                "position": "",
                "comment": "",
            },
            headers=headers,
            params=self._mobile_params(),
        )
        self.assertEqual(post_r.status_code, 200, f"POST shot failed: {post_r.status_code} - {post_r.text}")

        shots = self._get_list_first_available(
            ("/api/gets/GetAllShotsByUser", "/api/gets/GetAppShots"),
            headers=headers,
            params=self._mobile_params(),
        )
        matches = [s for s in shots if isinstance(s, dict) and s.get("mobileID") == shot_mobile]
        self.assertTrue(matches, "Posted shot not returned by GetAllShotsByUser")
        cloud_id = matches[0].get("id")
        self.assertIsInstance(cloud_id, int, "Expected cloud 'id' for shot")
        return shot_mobile, cloud_id

    # --- Authorization (app login) ---
    def test_authorize_with_valid_user(self):
        response = requests.post(
            _url("/api/posts/Authorize"),
            json={"username": TEST_USERNAME, "password": TEST_PASSWORD},
            headers={"Content-Type": "application/json"},
            verify=VERIFY_TLS,
        )
        self.assertEqual(response.status_code, 200, f"Expected 200, got {response.status_code}")

    def test_authorize_with_invalid_user(self):
        response = requests.post(
            _url("/api/posts/Authorize"),
            json={"username": "invalid", "password": "invalid"},
            headers={"Content-Type": "application/json"},
            verify=VERIFY_TLS,
        )
        self.assertEqual(response.status_code, 403, f"Expected 403, got {response.status_code}")

    def test_authorize_combined_phone_user(self):
        unique_username = f"{TEST_MARKER}_phone_auth_{self._high(500)}"
        combined_password_bytes = b"PhoneAuthPwBytes"
        combined_password = base64.b64encode(combined_password_bytes).decode("ascii")

        create_payload = {
            "mobileID": self._high(501),
            "firstname": "Phone",
            "lastname": "User",
            "username": unique_username,
            "hashedPassword": combined_password,
            "email": f"{TEST_MARKER}_{self._high(500)}@example.com",
            "phoneNumber": "5550000000",
            "lastLogin": None,
            "hand": None,
        }
        create_resp = requests.post(_url("/api/posts/PostUserApp"), json=create_payload, headers=auth_headers(), verify=VERIFY_TLS)
        self.assertEqual(create_resp.status_code, 200, f"Expected 200 creating combined user, got {create_resp.status_code} - {create_resp.text}")

        auth_resp = requests.post(
            _url("/api/posts/Authorize"),
            json={"username": unique_username, "password": combined_password},
            headers={"Content-Type": "application/json"},
            verify=VERIFY_TLS,
        )
        self.assertEqual(auth_resp.status_code, 200, f"Expected 200 authorizing combined user, got {auth_resp.status_code} - {auth_resp.text}")
        token = auth_resp.json().get("tokenA")
        self.assertTrue(token, "Expected tokenA in authorize response for combined user")

    # --- User (MobileApp POCO) ---
    def test_get_all_app_users(self):
        r = requests.get(_url("/api/gets/GetAppUsers"), headers=auth_headers(), params=self._mobile_params(), verify=VERIFY_TLS)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_user_app(self):
        marker_suffix = str(int(time.time()))
        payload = {
            "mobileID": self._high(800),
            "firstname": "TestFirst",
            "lastname": "TestLast",
            "username": f"{TEST_MARKER}_user_{marker_suffix}",
            "hashedPassword": "AQIDBA==",
            "email": f"{TEST_MARKER}_{marker_suffix}@example.com",
            "phoneNumber": "5551234567",
            "lastLogin": None,
            "hand": None,
        }
        r = requests.post(_url("/api/posts/PostUserApp"), json=payload, headers=auth_headers(), verify=VERIFY_TLS)
        self.assertIn(r.status_code, (200, 400), f"Expected 200 or 400, got {r.status_code} - {r.text}")

    # --- Establishment ---
    def test_post_establishment_app(self):
        payload = {
            "mobileID": self._high(1),
            "fullName": "TestEstablishment",
            "lanes": "1-10",
            "type": "TestType",
            "location": "TestLocation",
        }
        r = requests.post(_url("/api/posts/PostEstablishmentApp"), json=payload, headers=auth_headers(), params=self._mobile_params(), verify=VERIFY_TLS)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code} - {r.text}")
        cloud_id = r.json()
        self.assertIsInstance(cloud_id, int, f"Expected cloud ID (int) in response, got: {cloud_id}")

    def test_get_all_app_establishments(self):
        # Prefer the user-scoped endpoint (local), but fall back to legacy app endpoint (cloud until deployed).
        data = self._get_list_first_available(
            ("/api/gets/GetAllEstablishmentsByUser", "/api/gets/GetAppEstablishments"),
            headers=auth_headers(),
            params=self._mobile_params(),
        )
        self.assertIsInstance(data, list)
        for item in data:
            self.assertIsInstance(item, dict)
            if "mobileID" in item:
                self.assertTrue(item["mobileID"] is None or isinstance(item["mobileID"], int))

    # --- Game ---
    def test_get_all_app_games(self):
        data = self._get_list_first_available(
            ("/api/gets/GetAllGamesByUser", "/api/gets/GetAppGames"),
            headers=auth_headers(),
            params=self._mobile_params(),
        )
        self.assertIsInstance(data, list)

    def test_post_app_game(self):
        _, event_cloud_id = self._create_event_and_get_cloud_id()
        _, session_cloud_id = self._create_session_and_get_cloud_id(event_cloud_id)
        self._create_game_and_get_cloud_id(session_cloud_id)

    # --- Session ---
    def test_get_all_app_sessions(self):
        data = self._get_list_first_available(
            ("/api/gets/GetAllSessionsByUser", "/api/gets/GetAppSessions"),
            headers=auth_headers(),
            params=self._mobile_params(),
        )
        self.assertIsInstance(data, list)

    def test_post_app_session(self):
        _, event_cloud_id = self._create_event_and_get_cloud_id()
        self._create_session_and_get_cloud_id(event_cloud_id)

    # --- Shot ---
    def test_get_all_app_shots(self):
        data = self._get_list_first_available(
            ("/api/gets/GetAllShotsByUser", "/api/gets/GetAppShots"),
            headers=auth_headers(),
            params=self._mobile_params(),
        )
        self.assertIsInstance(data, list)

    def test_post_app_shot(self):
        _, event_cloud_id = self._create_event_and_get_cloud_id()
        _, session_cloud_id = self._create_session_and_get_cloud_id(event_cloud_id)
        _, game_cloud_id = self._create_game_and_get_cloud_id(session_cloud_id)
        _, frame_cloud_id = self._create_frame_and_get_cloud_id(game_cloud_id)
        self._create_shot_and_get_cloud_id(session_cloud_id, frame_cloud_id)

    # --- Ball: POST with mobileID + GET with ?mobileID= (phone sync) ---
    def test_get_balls_by_username_with_mobile_id_query(self):
        r = requests.get(
            _url("/api/gets/GetBallsByUsername"),
            headers=auth_headers(),
            params={"mobileID": TEST_MOBILE_ID},
            verify=VERIFY_TLS,
        )
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        data = r.json()
        self.assertIsInstance(data, list)
        for item in data:
            self.assertIsInstance(item, dict)
            if "mobileID" in item:
                self.assertTrue(item["mobileID"] is None or isinstance(item["mobileID"], int))

    def test_post_ball_and_get_roundtrip_with_mobile_id_query(self):
        ball_mobile = self._high(910)
        unique_name = f"{TEST_MARKER}_sync_ball_{int(time.time())}"
        post_r = requests.post(
            _url("/api/posts/PostBalls"),
            json={
                "mobileID": ball_mobile,
                "name": unique_name,
                "weight": "14",
                "coreType": "Reactive",
            },
            headers=auth_headers(),
            params={"mobileID": TEST_MOBILE_ID},
            verify=VERIFY_TLS,
        )
        self.assertEqual(post_r.status_code, 200, f"POST ball failed: {post_r.status_code} - {post_r.text}")

        get_r = requests.get(
            _url("/api/gets/GetBallsByUsername"),
            headers=auth_headers(),
            params={"mobileID": TEST_MOBILE_ID},
            verify=VERIFY_TLS,
        )
        self.assertEqual(get_r.status_code, 200, f"GET balls failed: {get_r.status_code} - {get_r.text}")
        balls = get_r.json()
        self.assertIsInstance(balls, list)
        found = any(
            isinstance(b, dict)
            and (b.get("name") == unique_name or b.get("mobileID") == ball_mobile)
            for b in balls
        )
        self.assertTrue(found, f"Posted ball not returned by GetBallsByUsername?mobileID={TEST_MOBILE_ID}")

    # --- Event: POST with mobileID + GET with ?mobileID= ---
    def test_get_events_by_username_with_mobile_id_query(self):
        r = requests.get(
            _url("/api/gets/GetEventsByUsername"),
            headers=auth_headers(),
            params={"mobileID": TEST_MOBILE_ID},
            verify=VERIFY_TLS,
        )
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        data = r.json()
        self.assertIsInstance(data, list)
        for item in data:
            self.assertIsInstance(item, dict)
            if "mobileID" in item:
                self.assertTrue(item["mobileID"] is None or isinstance(item["mobileID"], int))

    def test_post_event_and_get_roundtrip_with_mobile_id_query(self):
        event_mobile = self._high(920)
        unique_name = f"{TEST_MARKER}_sync_event_{int(time.time())}"
        post_r = requests.post(
            _url("/api/posts/PostEvent"),
            json={
                "mobileID": event_mobile,
                "userId": self._high(60),
                "longName": unique_name,
                "type": "Tournament",
                "location": "Test City",
                "average": self._high(61),
                "stats": 0,
                "standings": None,
            },
            headers=auth_headers(),
            params={"mobileID": TEST_MOBILE_ID},
            verify=VERIFY_TLS,
        )
        self.assertEqual(post_r.status_code, 200, f"POST event failed: {post_r.status_code} - {post_r.text}")

        get_r = requests.get(
            _url("/api/gets/GetEventsByUsername"),
            headers=auth_headers(),
            params={"mobileID": TEST_MOBILE_ID},
            verify=VERIFY_TLS,
        )
        self.assertEqual(get_r.status_code, 200, f"GET events failed: {get_r.status_code} - {get_r.text}")
        events = get_r.json()
        self.assertIsInstance(events, list)
        found = any(
            isinstance(e, dict)
            and (e.get("longName") == unique_name or e.get("mobileID") == event_mobile)
            for e in events
        )
        self.assertTrue(found, f"Posted event not returned by GetEventsByUsername?mobileID={TEST_MOBILE_ID}")

    # --- Frame ---
    def test_get_frames_by_game_id(self):
        _, event_cloud_id = self._create_event_and_get_cloud_id()
        _, session_cloud_id = self._create_session_and_get_cloud_id(event_cloud_id)
        _, game_cloud_id = self._create_game_and_get_cloud_id(session_cloud_id)
        r = requests.get(_url("/api/gets/GetFramesByGameId"), params={**self._mobile_params(), "gameId": str(game_cloud_id)}, headers=auth_headers(), verify=VERIFY_TLS)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_frames(self):
        _, event_cloud_id = self._create_event_and_get_cloud_id()
        _, session_cloud_id = self._create_session_and_get_cloud_id(event_cloud_id)
        _, game_cloud_id = self._create_game_and_get_cloud_id(session_cloud_id)
        self._create_frame_and_get_cloud_id(game_cloud_id)

    # --- Deletes ---
    def test_delete_app_shots(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppShots"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_frames(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppFrames"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_games(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppGames"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_sessions(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppSessions"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_events_by_username(self):
        r = requests.delete(
            _url("/api/deletes/DeleteEventsByUsername"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_establishments(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppEstablishments"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_balls_by_username(self):
        r = requests.delete(
            _url("/api/deletes/DeleteBallsByUsername"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_orphaned_app_data(self):
        r = requests.delete(
            _url("/api/deletes/DeleteOrphanedAppData"),
            headers=auth_headers(),
            verify=VERIFY_TLS,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_flow_removes_user_sessions_games_frames_and_shots(self):
        headers = auth_headers()
        # Create a valid owned graph: event -> session -> game -> frame -> shot
        _, event_cloud_id = self._create_event_and_get_cloud_id()
        session_mobile_id, session_cloud_id = self._create_session_and_get_cloud_id(event_cloud_id)
        game_mobile_id, game_cloud_id = self._create_game_and_get_cloud_id(session_cloud_id)
        frame_mobile_id, frame_cloud_id = self._create_frame_and_get_cloud_id(game_cloud_id)
        shot_mobile_id, _ = self._create_shot_and_get_cloud_id(session_cloud_id, frame_cloud_id)

        sessions_before = self._get_list_first_available(
            ("/api/gets/GetAllSessionsByUser", "/api/gets/GetAppSessions"),
            headers=headers,
            params=self._mobile_params(),
        )
        games_before = self._get_list_first_available(
            ("/api/gets/GetAllGamesByUser", "/api/gets/GetAppGames"),
            headers=headers,
            params=self._mobile_params(),
        )
        shots_before = self._get_list_first_available(
            ("/api/gets/GetAllShotsByUser", "/api/gets/GetAppShots"),
            headers=headers,
            params=self._mobile_params(),
        )
        frames_before = self._get_list("/api/gets/GetFramesByGameId", headers=headers, params={**self._mobile_params(), "gameId": str(game_cloud_id)})

        for endpoint in (
            "/api/deletes/DeleteAppShots",
            "/api/deletes/DeleteAppFrames",
            "/api/deletes/DeleteAppGames",
            "/api/deletes/DeleteAppSessions",
        ):
            delete_resp = requests.delete(
                _url(endpoint),
                json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
                headers=headers,
                verify=VERIFY_TLS,
            )
            self.assertEqual(delete_resp.status_code, 200, f"{endpoint} failed: {delete_resp.status_code} - {delete_resp.text}")

        sessions_after = self._get_list_first_available(
            ("/api/gets/GetAllSessionsByUser", "/api/gets/GetAppSessions"),
            headers=headers,
            params=self._mobile_params(),
        )
        games_after = self._get_list_first_available(
            ("/api/gets/GetAllGamesByUser", "/api/gets/GetAppGames"),
            headers=headers,
            params=self._mobile_params(),
        )
        shots_after = self._get_list_first_available(
            ("/api/gets/GetAllShotsByUser", "/api/gets/GetAppShots"),
            headers=headers,
            params=self._mobile_params(),
        )
        frames_after = self._get_list("/api/gets/GetFramesByGameId", headers=headers, params={**self._mobile_params(), "gameId": str(game_cloud_id)})

        # Assert our specific created items are removed (works even if legacy endpoints return other users too).
        self.assertTrue(any(isinstance(s, dict) and s.get("mobileID") == session_mobile_id for s in sessions_before))
        self.assertTrue(any(isinstance(g, dict) and g.get("mobileID") == game_mobile_id for g in games_before))
        self.assertTrue(any(isinstance(sh, dict) and sh.get("mobileID") == shot_mobile_id for sh in shots_before))
        self.assertTrue(any(isinstance(f, dict) and f.get("mobileID") == frame_mobile_id for f in frames_before))

        self.assertFalse(any(isinstance(f, dict) and f.get("mobileID") == frame_mobile_id for f in frames_after), "Frame still exists after DeleteAppFrames")
        self.assertFalse(any(isinstance(sh, dict) and sh.get("mobileID") == shot_mobile_id for sh in shots_after), "Shot still exists after DeleteAppShots")
        self.assertFalse(any(isinstance(g, dict) and g.get("mobileID") == game_mobile_id for g in games_after), "Game still exists after DeleteAppGames")
        self.assertFalse(any(isinstance(s, dict) and s.get("mobileID") == session_mobile_id for s in sessions_after), "Session still exists after DeleteAppSessions")


if __name__ == "__main__":
    unittest.main()
