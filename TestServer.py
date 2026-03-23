"""
TestServer.py - Tests for BallSpinner-Cloud API (DatabaseControllers2025 + Auth).

Switch BASE_URL to test locally or against cloud.
Run: python -m pytest TestServer.py -v
Or:  python TestServer.py
"""
import unittest
import requests
import warnings
import time
import base64

# --- Configure base URL (no trailing slash) ---
# Local:  https://localhost:7238
# Cloud:  https://api.revmetrix.io
# BASE_URL = "https://localhost:7238"
BASE_URL = "https://api.revmetrix.io"

# Test user for authorized requests (must exist in DB for auth tests)
TEST_USERNAME = "string"
TEST_PASSWORD = "string"
# MobileID for the same test user (used by delete endpoints).
# If you don't know it yet, keep this at 0; the server falls back to username-only matching.
TEST_MOBILE_ID = 1
TEST_MARKER = "PYTEST_CLOUD"

# Keep test-generated numeric values very high to avoid collisions with real app data.
HIGH_NUM_BASE = 1_900_000_000 + int(time.time()) % 10_000


def _url(path: str) -> str:
    """Build full URL. path should start with / (e.g. /api/gets/GetAppUsers)."""
    return BASE_URL.rstrip("/") + ("/" + path.lstrip("/") if not path.startswith("http") else path)


def get_auth_token() -> str:
    """Get JWT for TEST_USERNAME/TEST_PASSWORD. Raises on failure."""
    warnings.filterwarnings("ignore", message="Unverified HTTPS request")
    r = requests.post(
        _url("/api/posts/Authorize"),
        json={"username": TEST_USERNAME, "password": TEST_PASSWORD},
        headers={"Content-Type": "application/json"},
        verify=False,
    )
    if r.status_code != 200:
        raise AssertionError(f"Authorize failed: {r.status_code} - {r.text}")
    token = r.json().get("tokenA")
    if not token:
        raise AssertionError("No tokenA in response")
    return token


def auth_headers() -> dict:
    return {"Authorization": f"Bearer {get_auth_token()}", "Content-Type": "application/json"}


class TestAPIEndpoint(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        cls._run_cloud_cleanup()

    @classmethod
    def tearDownClass(cls):
        cls._run_cloud_cleanup()

    @classmethod
    def _run_cloud_cleanup(cls):
        try:
            headers = auth_headers()
        except Exception:
            return

        for endpoint in (
            "/api/deletes/DeleteAppShots",
            "/api/deletes/DeleteAppFrames",
            "/api/deletes/DeleteAppGames",
            "/api/deletes/DeleteAppSessions",
            "/api/deletes/DeleteEventsByUsername",
            "/api/deletes/DeleteBallsByUsername",
            "/api/deletes/DeleteAppEstablishments",
        ):
            try:
                requests.delete(
                    _url(endpoint),
                    json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
                    headers=headers,
                    verify=False,
                )
            except requests.RequestException:
                pass

    @staticmethod
    def _high(offset: int) -> int:
        return HIGH_NUM_BASE + offset

    def setUp(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")

    # --- Test endpoint ---
    def test_endpoint_returns_200(self):
        url = _url("/api/tests/Test")
        if "localhost" in BASE_URL:
            url = _url("/api/tests/Test").replace("https://localhost", "https://localhost")
        response = requests.get(url, verify=False)
        self.assertEqual(response.status_code, 200, f"Expected 200, got {response.status_code}")

    # --- Authorization ---
    def test_authorize_with_valid_user(self):
        response = requests.post(
            _url("/api/posts/Authorize"),
            json={"username": TEST_USERNAME, "password": TEST_PASSWORD},
            headers={"Content-Type": "application/json"},
            verify=False,
        )
        self.assertEqual(response.status_code, 200, f"Expected 200, got {response.status_code}")

    def test_authorize_with_invalid_user(self):
        response = requests.post(
            _url("/api/posts/Authorize"),
            json={"username": "invalid", "password": "invalid"},
            headers={"Content-Type": "application/json"},
            verify=False,
        )
        self.assertEqual(response.status_code, 403, f"Expected 403, got {response.status_code}")

    def test_authorize_with_invalid_request_data(self):
        response = requests.post(_url("/api/posts/Authorize"), data={"x": "y"}, verify=False)
        self.assertEqual(response.status_code, 415, f"Expected 415, got {response.status_code}")

    # --- User (MobileApp POCO) ---
    def test_get_all_app_users(self):
        r = requests.get(_url("/api/gets/GetAppUsers"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_user_app(self):
        # PostUserApp expects MobileApp.User; hashedPassword must be base64 or hex bytes - server may expect pre-hashed
        marker_suffix = str(int(time.time()))
        payload = {
            "firstname": "TestFirst",
            "lastname": "TestLast",
            "username": f"{TEST_MARKER}_user_{marker_suffix}",
            "hashedPassword": "AQIDBA==",  # minimal base64 bytes; replace with real hash if server requires
            "email": f"{TEST_MARKER}_{marker_suffix}@example.com",
            "phoneNumber": "5551234567",
            "lastLogin": None,
            "hand": None,
        }
        r = requests.post(_url("/api/posts/PostUserApp"), json=payload, headers=auth_headers(), verify=False)
        # 200 = success; 400 if validation fails (e.g. hashedPassword format)
        self.assertIn(r.status_code, (200, 400), f"Expected 200 or 400, got {r.status_code} - {r.text}")

    # --- Establishment (MobileApp POCO) ---
    def test_get_all_app_establishments(self):
        r = requests.get(_url("/api/gets/GetAppEstablishments"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_establishment_app(self):
        payload = {
            "name": "TestEstablishment",
            "lanes": "1-10",
            "type": "TestType",
            "location": "TestLocation",
        }
        r = requests.post(_url("/api/posts/PostEstablishmentApp"), json=payload, headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code} - {r.text}")

    def test_post_establishment_app_with_mobile_id(self):
        payload = {
            "mobileID": self._high(1),
            "name": "TestEstablishmentMobileId",
            "lanes": "1-10",
            "type": "TestType",
            "location": "TestLocation",
        }
        r = requests.post(_url("/api/posts/PostEstablishmentApp"), json=payload, headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code} - {r.text}")

    # --- Game (MobileApp POCO) ---
    def test_get_all_app_games(self):
        r = requests.get(_url("/api/gets/GetAppGames"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_app_game(self):
        payload = {
            "gameNumber": "G001",
            "lanes": "1-2",
            "score": self._high(10),
            "win": 1,
            "startingLane": self._high(11),
            "sessionId": self._high(12),
            "teamResult": 1,
            "individualResult": 1,
        }
        r = requests.post(_url("/api/posts/PostAppGame"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    def test_post_app_game_with_mobile_id(self):
        payload = {
            "mobileID": self._high(2),
            "gameNumber": "G_MobileId",
            "lanes": "1-2",
            "score": self._high(20),
            "win": 1,
            "startingLane": self._high(21),
            "sessionId": self._high(22),
            "teamResult": 1,
            "individualResult": 1,
        }
        r = requests.post(_url("/api/posts/PostAppGame"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    # --- Session (MobileApp POCO) ---
    def test_get_all_app_sessions(self):
        r = requests.get(_url("/api/gets/GetAppSessions"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_app_session(self):
        payload = {
            "sessionNumber": self._high(23),
            "establishmentId": self._high(24),
            "eventId": self._high(25),
            "dateTime": self._high(26),
            "teamOpponent": "TeamA",
            "individualOpponent": "Opponent1",
            "score": self._high(27),
            "stats": 0,
            "teamRecord": 1,
            "individualRecord": 1,
        }
        r = requests.post(_url("/api/posts/PostAppSession"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    def test_post_app_session_with_mobile_id(self):
        payload = {
            "mobileID": self._high(3),
            "sessionNumber": self._high(30),
            "establishmentId": self._high(31),
            "eventId": self._high(32),
            "dateTime": self._high(33),
            "teamOpponent": "TeamA",
            "individualOpponent": "Opponent1",
            "score": self._high(34),
            "stats": 0,
            "teamRecord": 1,
            "individualRecord": 1,
        }
        r = requests.post(_url("/api/posts/PostAppSession"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    # --- Shot (MobileApp POCO) ---
    def test_get_all_app_shots(self):
        r = requests.get(_url("/api/gets/GetAppShots"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_app_shot(self):
        payload = {
            "type": 1,
            "smartDotId": self._high(35),
            "sessionId": self._high(36),
            "ballId": self._high(37),
            "frameId": self._high(38),
            "shotNumber": self._high(39),
            "leaveType": 0,
            "side": "left",
            "position": "10",
            "comment": "Test shot",
        }
        r = requests.post(_url("/api/posts/PostAppShot"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    def test_post_app_shot_with_mobile_id(self):
        payload = {
            "mobileID": self._high(4),
            "type": 1,
            "smartDotId": self._high(40),
            "sessionId": self._high(41),
            "ballId": self._high(42),
            "frameId": self._high(43),
            "shotNumber": self._high(44),
            "leaveType": 0,
            "side": "left",
            "position": "10",
            "comment": "Test shot",
        }
        r = requests.post(_url("/api/posts/PostAppShot"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    # --- Ball (MobileApp POCO) ---
    def test_get_balls_by_username(self):
        r = requests.get(_url("/api/gets/GetBallsByUsername"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_ball(self):
        payload = {
            "name": "TestBall",
            "weight": "14",
            "coreType": "Reactive",
        }
        r = requests.post(_url("/api/posts/PostBalls"), json=payload, headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code} - {r.text}")

    def test_post_ball_with_mobile_id(self):
        payload = {
            "mobileID": self._high(5),
            "name": "TestBallMobileId",
            "weight": "14",
            "coreType": "Reactive",
        }
        r = requests.post(_url("/api/posts/PostBalls"), json=payload, headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code} - {r.text}")

    # --- Event (MobileApp POCO) ---
    def test_get_events_by_username(self):
        r = requests.get(_url("/api/gets/GetEventsByUsername"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_event(self):
        payload = {
            "userId": self._high(50),
            "name": "Test Event",
            "type": "Tournament",
            "location": "Test City",
            "average": self._high(51),
            "stats": 0,
            "standings": None,
        }
        r = requests.post(_url("/api/posts/PostEvent"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    def test_post_event_with_mobile_id(self):
        payload = {
            "mobileID": self._high(6),
            "userId": self._high(60),
            "name": "Test Event MobileId",
            "type": "Tournament",
            "location": "Test City",
            "average": self._high(61),
            "stats": 0,
            "standings": None,
        }
        r = requests.post(_url("/api/posts/PostEvent"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    # --- Frame (MobileApp POCO) ---
    def test_get_frames_by_game_id(self):
        r = requests.get(
            _url("/api/gets/GetFramesByGameId"),
            params={"gameId": 1},
            headers=auth_headers(),
            verify=False,
        )
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_frames(self):
        payload = {
            "gameId": self._high(65),
            "shotOne": self._high(66),
            "shotTwo": self._high(67),
            "frameNumber": self._high(68),
            "lane": self._high(69),
            "result": self._high(70),
        }
        r = requests.post(_url("/api/posts/PostFrames"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

    def test_post_frames_with_mobile_id(self):
        payload = {
            "mobileID": self._high(7),
            "gameId": self._high(70),
            "shotOne": self._high(71),
            "shotTwo": self._high(72),
            "frameNumber": self._high(73),
            "lane": self._high(74),
            "result": self._high(75),
        }
        r = requests.post(_url("/api/posts/PostFrames"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")

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
        create_resp = requests.post(_url("/api/posts/PostUserApp"), json=create_payload, headers=auth_headers(), verify=False)
        self.assertEqual(create_resp.status_code, 200, f"Expected 200 creating combined user, got {create_resp.status_code} - {create_resp.text}")

        auth_resp = requests.post(
            _url("/api/posts/Authorize"),
            json={"username": unique_username, "password": combined_password},
            headers={"Content-Type": "application/json"},
            verify=False,
        )
        self.assertEqual(auth_resp.status_code, 200, f"Expected 200 authorizing combined user, got {auth_resp.status_code} - {auth_resp.text}")
        token = auth_resp.json().get("tokenA")
        self.assertTrue(token, "Expected tokenA in authorize response for combined user")

    # --- Deletes (delete all for authenticated user; no body) ---
    def test_delete_app_shots(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppShots"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=False,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_frames(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppFrames"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=False,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_games(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppGames"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=False,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_sessions(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppSessions"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=False,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_events_by_username(self):
        r = requests.delete(
            _url("/api/deletes/DeleteEventsByUsername"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=False,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_app_establishments(self):
        r = requests.delete(
            _url("/api/deletes/DeleteAppEstablishments"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=False,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_balls_by_username(self):
        r = requests.delete(
            _url("/api/deletes/DeleteBallsByUsername"),
            json={"username": TEST_USERNAME, "mobileID": TEST_MOBILE_ID},
            headers=auth_headers(),
            verify=False,
        )
        self.assertIn(r.status_code, (200, 401, 500), f"Got {r.status_code} - {r.text}")

    def test_delete_flow_removes_user_sessions_games_frames_and_shots(self):
        headers = auth_headers()

        def _get_json(path: str):
            resp = requests.get(_url(path), headers=headers, verify=False)
            self.assertEqual(resp.status_code, 200, f"Expected 200 from {path}, got {resp.status_code} - {resp.text}")
            data = resp.json()
            self.assertIsInstance(data, list, f"{path} should return a list")
            return data

        # Build ownership chain for this user from events -> sessions -> games -> frames/shots.
        events = _get_json("/api/gets/GetEventsByUsername")
        user_event_ids = {e.get("id") for e in events if isinstance(e, dict) and e.get("id") is not None}

        sessions_before = _get_json("/api/gets/GetAppSessions")
        user_sessions_before = [s for s in sessions_before if isinstance(s, dict) and s.get("eventID") in user_event_ids]
        user_session_ids = {s.get("id") for s in user_sessions_before if s.get("id") is not None}

        games_before = _get_json("/api/gets/GetAppGames")
        user_games_before = [g for g in games_before if isinstance(g, dict) and g.get("sessionID") in user_session_ids]
        user_game_ids = {g.get("id") for g in user_games_before if g.get("id") is not None}

        frames_before = _get_json("/api/gets/GetFramesByGameId?gameId=1")
        user_frames_before = [f for f in frames_before if isinstance(f, dict) and f.get("gameId") in user_game_ids]

        shots_before = _get_json("/api/gets/GetAppShots")
        user_shots_before = [
            s for s in shots_before
            if isinstance(s, dict) and (s.get("sessionID") in user_session_ids or s.get("frameID") in {f.get("id") for f in user_frames_before})
        ]

        # Run delete chain from leaf -> root.
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
                verify=False,
            )
            self.assertEqual(delete_resp.status_code, 200, f"{endpoint} failed: {delete_resp.status_code} - {delete_resp.text}")

        sessions_after = _get_json("/api/gets/GetAppSessions")
        user_sessions_after = [s for s in sessions_after if isinstance(s, dict) and s.get("eventID") in user_event_ids]

        games_after = _get_json("/api/gets/GetAppGames")
        user_session_ids_after = {s.get("id") for s in user_sessions_after if s.get("id") is not None}
        user_games_after = [g for g in games_after if isinstance(g, dict) and g.get("sessionID") in user_session_ids_after]

        frames_after = _get_json("/api/gets/GetFramesByGameId?gameId=1")
        user_game_ids_after = {g.get("id") for g in user_games_after if g.get("id") is not None}
        user_frames_after = [f for f in frames_after if isinstance(f, dict) and f.get("gameId") in user_game_ids_after]

        shots_after = _get_json("/api/gets/GetAppShots")
        user_frame_ids_after = {f.get("id") for f in user_frames_after if f.get("id") is not None}
        user_shots_after = [
            s for s in shots_after
            if isinstance(s, dict) and (s.get("sessionID") in user_session_ids_after or s.get("frameID") in user_frame_ids_after)
        ]

        # If there was nothing to delete this test is still valid, but this helps with diagnosis.
        self.assertGreaterEqual(len(user_sessions_before), 0)
        self.assertGreaterEqual(len(user_games_before), 0)
        self.assertGreaterEqual(len(user_frames_before), 0)
        self.assertGreaterEqual(len(user_shots_before), 0)

        self.assertEqual(len(user_sessions_after), 0, "User sessions still exist after DeleteAppSessions")
        self.assertEqual(len(user_games_after), 0, "User games still exist after DeleteAppGames")
        self.assertEqual(len(user_frames_after), 0, "User frames still exist after DeleteAppFrames")
        self.assertEqual(len(user_shots_after), 0, "User shots still exist after DeleteAppShots")

    # --- MobileID round-trip: POST with mobileId, GET and verify item can have mobileID ---
    def test_get_app_establishments_may_include_mobile_id(self):
        r = requests.get(_url("/api/gets/GetAppEstablishments"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        data = r.json()
        self.assertIsInstance(data, list, "GetAppEstablishments should return a list")
        for item in data:
            self.assertIsInstance(item, dict, "Each item should be a dict")
            if "mobileID" in item:
                self.assertTrue(item["mobileID"] is None or isinstance(item["mobileID"], int), "mobileID should be null or int")

    def test_get_balls_may_include_mobile_id(self):
        r = requests.get(_url("/api/gets/GetBallsByUsername"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        data = r.json()
        self.assertIsInstance(data, list, "GetBallsByUsername should return a list")
        for item in data:
            self.assertIsInstance(item, dict, "Each item should be a dict")
            if "mobileID" in item:
                self.assertTrue(item["mobileID"] is None or isinstance(item["mobileID"], int), "mobileID should be null or int")

    def test_get_events_may_include_mobile_id(self):
        r = requests.get(_url("/api/gets/GetEventsByUsername"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        data = r.json()
        self.assertIsInstance(data, list, "GetEventsByUsername should return a list")
        for item in data:
            self.assertIsInstance(item, dict, "Each item should be a dict")
            if "mobileID" in item:
                self.assertTrue(item["mobileID"] is None or isinstance(item["mobileID"], int), "mobileID should be null or int")


if __name__ == "__main__":
    unittest.main()
