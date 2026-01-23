import requests
import sys
from pathlib import Path

BASE = "https://api.revmetrix.io"  # change to "https://api.revmetrix.io" when deployed or https://localhost:7238/api
VERIFY = True  # set True when using a valid cert in production


def fail(msg, resp=None):
    if resp is not None:
        print(f"{msg}: status={resp.status_code}, body={resp.text}")
    else:
        print(msg)
    sys.exit(1)


def authorize(username: str, password: str) -> str:
    resp = requests.post(
        f"{BASE}/api/posts/Authorize",
        json={"username": username, "password": password},
        headers={"Content-Type": "application/json"},
        verify=VERIFY,
    )
    if resp.status_code != 200:
        fail("Authorize failed", resp)
    data = resp.json()
    token = data.get("tokenA")
    if not token:
        fail("Authorize returned no tokenA", resp)
    print("Authorize OK")
    return token


def upload_video(token: str, file_path: Path, folder: str = "videos") -> str:
    if not file_path.exists():
        fail(f"File not found: {file_path}")

    headers = {"Authorization": f"Bearer {token}"}
    with file_path.open("rb") as f:
        files = {"file": (file_path.name, f, "video/mp4")}
        resp = requests.post(
            f"{BASE}/api/videos/upload",
            params={"folder": folder},
            headers=headers,
            files=files,
            verify=VERIFY,
        )
    if resp.status_code != 200:
        fail("Upload failed", resp)
    key = resp.json().get("key")
    if not key:
        fail("Upload response missing 'key'", resp)
    print(f"Upload OK, key={key}")
    return key


def get_presigned_url(token: str, key: str, ttl_seconds: int = 600) -> str:
    headers = {"Authorization": f"Bearer {token}"}
    resp = requests.get(
        f"{BASE}/videos/presign",
        params={"key": key, "ttlSeconds": ttl_seconds},
        headers=headers,
        verify=VERIFY,
    )
    if resp.status_code != 200:
        fail("Presign failed", resp)
    url = resp.json().get("url")
    if not url:
        fail("Presign response missing 'url'", resp)
    print("Presign OK")
    return url


def download_from_spaces(url: str, out_path: Path):
    resp = requests.get(url, stream=True)
    if resp.status_code != 200:
        fail("Spaces download failed", resp)
    with out_path.open("wb") as out:
        for chunk in resp.iter_content(chunk_size=8192):
            if chunk:
                out.write(chunk)
    print(f"Downloaded to {out_path}")


def main():
    # Adjust these for your environment
    username = "string"
    password = "string"

    # Input video to upload (place a small test file next to the repo root)
    input_video = Path("test.mp4")
    output_video = Path("downloaded_test.mp4")

    # 1) Authorize
    token = authorize(username, password)

    # 2) Upload
    key = upload_video(token, input_video, folder="videos")

    # 3) Get presigned URL
    url = get_presigned_url(token, key, ttl_seconds=600)

    # 4) Download directly from Spaces
    download_from_spaces(url, output_video)

    print("End-to-end video flow OK")


if __name__ == "__main__":
    main()