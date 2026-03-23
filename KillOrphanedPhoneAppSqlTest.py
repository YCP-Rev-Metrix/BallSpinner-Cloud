"""
KillOrphanedPhoneAppSqlTest.py - Focused test for orphan cleanup endpoint.

Run:
  python -m pytest KillOrphanedPhoneAppSqlTest.py -v
or:
  python KillOrphanedPhoneAppSqlTest.py
"""

import unittest
import requests
import warnings

# --- Configure base URL (no trailing slash) ---
# Local:  https://localhost:7238
# Cloud:  https://api.revmetrix.io
# BASE_URL = "https://localhost:7238"
BASE_URL = "https://api.revmetrix.io"

# Auth user must exist and be allowed to call authorized deletes.
TEST_USERNAME = "string"
TEST_PASSWORD = "string"


def _url(path: str) -> str:
    return BASE_URL.rstrip("/") + ("/" + path.lstrip("/") if not path.startswith("http") else path)


def get_auth_token() -> str:
    warnings.filterwarnings("ignore", message="Unverified HTTPS request")
    response = requests.post(
        _url("/api/posts/Authorize"),
        json={"username": TEST_USERNAME, "password": TEST_PASSWORD},
        headers={"Content-Type": "application/json"},
        verify=False,
    )
    if response.status_code != 200:
        raise AssertionError(f"Authorize failed: {response.status_code} - {response.text}")
    token = response.json().get("tokenA")
    if not token:
        raise AssertionError("No tokenA in authorize response")
    return token


def auth_headers() -> dict:
    return {"Authorization": f"Bearer {get_auth_token()}", "Content-Type": "application/json"}


class TestKillOrphanedPhoneAppSql(unittest.TestCase):
    def setUp(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")

    def test_delete_orphaned_app_data_endpoint(self):
        response = requests.delete(
            _url("/api/deletes/DeleteOrphanedAppData"),
            headers=auth_headers(),
            verify=False,
        )
        self.assertEqual(
            response.status_code,
            200,
            f"Expected 200 from DeleteOrphanedAppData, got {response.status_code} - {response.text}",
        )


if __name__ == "__main__":
    unittest.main()

