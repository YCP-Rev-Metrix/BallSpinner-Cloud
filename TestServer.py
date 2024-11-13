import unittest
import requests

import warnings

class TestAPIEndpoint(unittest.TestCase):
    
    # Test endpoint
    def test_endpoint_returns_200(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/tests/Test"
        response = requests.get(url, verify=False)
        self.assertEqual(response.status_code, 200, f"Expected 200, but got {response.status_code}")
    
    ## AUTHOIRZATION TESTS
    def test_authorize_with_valid_user(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Authorize"
        payload = {
            "username": "string",
            "password": "string"
        }
        headers = {'Content-Type': 'application/json'}
        response = requests.post(url, json=payload, headers = headers, verify=False)
        self.assertEqual(response.status_code, 200, f"Expected 200, but got {response.status_code}")

    def test_authorize_with_invalid_user(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Authorize"
        payload = {
            'username': 'invalid',
            'password': 'invalid'
        }
        response = requests.post(url, json=payload,verify=False)
        self.assertEqual(response.status_code, 403, f"Expected 403, but got {response.status_code}")

    def test_authorize_with_invalid_request_data(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Authorize"
        payload = {
            'invalid': 'invalid',
            'invalid': 'invalid',
        }
        response = requests.post(url, data=payload,verify=False) #Testing with unsupported media type
        self.assertEqual(response.status_code, 415, f"Expected 415, but got {response.status_code}")

    ## REGISTER TESTS
    def test_register_with_valid_data(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Register"
        payload = {
            "firstname": "sup",
            "lastname": "sup",
            "username": "sup",
            "password": "sup",
            "email": "sup@example.com",
            "phoneNumber": "sup"
        }
        response = requests.post(url, json=payload,verify=False)
        self.assertEqual(response.status_code, 200, f"Expected 200, but got {response.status_code}")

    def test_register_with_duplicate_user(self): # This should return 403 because it is the same as the initial test user
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Register"
        payload = {
            "firstname": "string",
            "lastname": "string",
            "username": "string",
            "password": "string",
            "email": "string@example.com",
            "phoneNumber": "string"
        }
        response = requests.post(url, json=payload,verify=False)
        self.assertEqual(response.status_code, 500, f"Expected 500, but got {response.status_code}")

    def test_register_with_invalid_request_data(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Register"
        payload = {
            "firstname": "hi",
            "lastname": "hi",
            "username": "hi",
            "password": "hi",
            "email": "hi@example.com",
            "phoneNumber": "hi"
        }
        response = requests.post(url, data=payload,verify=False) # Not JSON encoded, so request data is invalid
        self.assertEqual(response.status_code, 415, f"Expected 415, but got {response.status_code}")


if __name__ == "__main__":
    unittest.main()
