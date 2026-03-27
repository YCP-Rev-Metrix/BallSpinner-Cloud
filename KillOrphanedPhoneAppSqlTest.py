"""
KillOrphanedPhoneAppSqlTest.py - Focused test for orphan cleanup endpoint.

Run:
  python -m pytest KillOrphanedPhoneAppSqlTest.py -v
or:
  python KillOrphanedPhoneAppSqlTest.py
"""

import unittest
import warnings

import requests

# --- Configure base URL (no trailing slash) ---
# Local:  https://localhost:7238
# Cloud:  https://api.revmetrix.io
# BASE_URL = "https://localhost:7238"
BASE_URL = "https://api.revmetrix.io"

# Auth user must exist and be allowed to call authorized deletes.
TEST_USERNAME = "string"
TEST_PASSWORD = "string"

import TestServer

# Cached auth in TestServer uses its module globals — keep them aligned with this file.
TestServer.BASE_URL = BASE_URL
TestServer.VERIFY_TLS = False
TestServer.TEST_USERNAME = TEST_USERNAME
TestServer.TEST_PASSWORD = TEST_PASSWORD

from TestServer import auth_headers


def _url(path: str) -> str:
    return BASE_URL.rstrip("/") + ("/" + path.lstrip("/") if not path.startswith("http") else path)


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

