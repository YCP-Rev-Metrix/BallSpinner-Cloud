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
            "firstname": "newUser",
            "lastname": "newUser",
            "username": "newUser",
            "password": "newUser",
            "email": "newUser@example.com",
            "phoneNumber": "123213212"
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


    def test_get_shot_by_username(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Authorize"
        payload = {
            'username': 'string',
            'password': 'string'
        }
        response = requests.post(url, json=payload,verify=False)
        token = response.json().get("tokenA")

        url = "https://localhost:7238/api/gets/GetShotsByUsername"
        headers = {
                "Authorization": f"Bearer {token}"
                }

        
        response = requests.get(url, json=payload,verify=False,headers=headers) # Not JSON encoded, so request data is invalid
        self.assertEqual(response.status_code, 200, f"Expected 200, but got {response.status_code}")

    def test_insert_simulated_shot(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Authorize"
        payload = {
            'username': 'string',
            'password': 'string'
        }
        response = requests.post(url, json=payload,verify=False)
        token = response.json().get("tokenA")

        url = "https://localhost:7238/api/posts/InsertSimulatedShot"
        headers = {
                "Authorization": f"Bearer {token}",
                }
        payload = {
            "simulatedShot": {
                "name": "pytestshot",
                "speed": 12.3,
                "angle": 51.1,
                "position": 12.1,
                "frequency": 25
            },
            "data": [
                {
                    "type": "1",
                    "count": 2,
                    "logtime": 12,
                    "x": 41,
                    "y": 21,
                    "z": 12
                },
                {
                    "type": "2",
                    "count": 2,
                    "logtime": 12,
                    "x": 41,
                    "y": 21,
                    "z": 12
                },
                {
                    "type": "3",
                    "count": 2,
                    "logtime": 12,
                    "x": 41,
                    "y": 21,
                    "z": 12
                },
                {
                    "type": "4",
                    "count": 2,
                    "logtime": 12,
                    "x": 41,
                    "y": 21,
                    "z": 12
                }
            ]
        }


        response = requests.post(url, json=payload,verify=False,headers=headers) # Not JSON encoded, so request data is invalid
        self.assertEqual(response.status_code, 200, f"Expected 200, but got {response.status_code}")

    def test_insert_simulated_shot_invalid(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Authorize"
        payload = {
            'username': 'string',
            'password': 'string'
        }
        response = requests.post(url, json=payload,verify=False)
        token = response.json().get("tokenA")

        url = "https://localhost:7238/api/posts/InsertSimulatedShot"
        headers = {
                "Authorization": f"Bearer {token}",
                }
        payload = {
            "simulatedShot": {
                "name": "newShot",
                "speed": 12.3,
                "angle": 51.1,
                "position": 12.1,
                "frequency": 25
            },
            "data": [
                {
                    "type": "Lightsensor",
                    "count": 2,
                    "logtime": 12,
                    "x": 41,
                    "y": 21,
                    "z": 12,
                    "w": 32
                }
            ]
        }


        response = requests.post(url, json=payload,verify=False,headers=headers)
        self.assertEqual(response.status_code, 403, f"Expected 403, but got {response.status_code}")

    def test_get_all_shots(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Authorize"
        payload = {
            'username': 'string',
            'password': 'string'
        }
        response = requests.post(url, json=payload, verify=False)
        token = response.json().get("tokenA")

        url = "https://localhost:7238/api/gets/GetAllShots"
        headers = {
                "Authorization": f"Bearer {token}"
                }

        response = requests.get(url, verify=False, headers=headers)
        self.assertEqual(response.status_code, 200, f"Expected 200, but got {response.status_code}")

    def test_get_all_shots_invalid(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/posts/Register"
        payload = {
            "firstname": "testing",
            "lastname": "testing",
            "username": "testing",
            "password": "testing",
            "email": "testing@example.com",
            "phoneNumber": "testing"
        }
        response = requests.post(url, data=payload,verify=False) # Not JSON encoded, so request data is invalid
        token = response.json().get("tokenA")

        url = "https://localhost:7238/api/gets/GetAllShots"
        headers = {
                "Authorization": f"Bearer {token}"
                }

        response = requests.get(url, verify=False, headers=headers)
        self.assertEqual(response.status_code, 401, f"Expected 401, but got {response.status_code}")

    def test_get_userid(self):
        warnings.filterwarnings("ignore", message="Unverified HTTPS request")
        url = "https://localhost:7238/api/gets/GetUserId"

        query = {
            "username": "string"
        }

        response = requests.get(url, verify=False,params=query)
        self.assertEqual(response.status_code, 200, f"Expected 200, but got {response.status_code}")


if __name__ == "__main__":
    unittest.main()
