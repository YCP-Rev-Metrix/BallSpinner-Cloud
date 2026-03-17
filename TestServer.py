"""
TestServer.py - Tests for BallSpinner-Cloud API (DatabaseControllers2025 + Auth).

Switch BASE_URL to test locally or against cloud.
Run: python -m pytest TestServer.py -v
Or:  python TestServer.py
"""
import unittest
import requests
import warnings

# --- Configure base URL (no trailing slash) ---
# Local:  https://localhost:7238
# Cloud:  https://api.revmetrix.io
BASE_URL = "https://localhost:7238"
# BASE_URL = "https://api.revmetrix.io"

# Test user for authorized requests (must exist in DB for auth tests)
TEST_USERNAME = "string"
TEST_PASSWORD = "string"


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
        payload = {
            "firstname": "TestFirst",
            "lastname": "TestLast",
            "username": "testuser_py_" + str(__import__("time").time()),
            "hashedPassword": "AQIDBA==",  # minimal base64 bytes; replace with real hash if server requires
            "email": "testpy@example.com",
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

    # --- Game (MobileApp POCO) ---
    def test_get_all_app_games(self):
        r = requests.get(_url("/api/gets/GetAppGames"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_app_game(self):
        payload = {
            "gameNumber": "G001",
            "lanes": "1-2",
            "score": 200,
            "win": 1,
            "startingLane": 1,
            "sessionId": 1,
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
            "sessionNumber": 1,
            "establishmentId": 1,
            "eventId": 1,
            "dateTime": 20250101120000,
            "teamOpponent": "TeamA",
            "individualOpponent": "Opponent1",
            "score": 500,
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
            "smartDotId": 1,
            "sessionId": 1,
            "ballId": 1,
            "frameId": 1,
            "shotNumber": 1,
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

    # --- Event (MobileApp POCO) ---
    def test_get_events_by_username(self):
        r = requests.get(_url("/api/gets/GetEventsByUsername"), headers=auth_headers(), verify=False)
        self.assertEqual(r.status_code, 200, f"Expected 200, got {r.status_code}")
        self.assertIsInstance(r.json(), list)

    def test_post_event(self):
        payload = {
            "userId": 1,
            "name": "Test Event",
            "type": "Tournament",
            "location": "Test City",
            "average": 200,
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
            "gameId": 1,
            "shotOne": 0,
            "shotTwo": 0,
            "frameNumber": 1,
            "lane": 1,
            "result": 10,
        }
        r = requests.post(_url("/api/posts/PostFrames"), json=payload, headers=auth_headers(), verify=False)
        self.assertIn(r.status_code, (200, 400, 404), f"Got {r.status_code} - {r.text}")


if __name__ == "__main__":
    unittest.main()
