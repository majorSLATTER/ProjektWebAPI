import requests

apiUrl = "https://localhost:7148/"

Trailer1 = {
    "name":"Big Bertha",
    "weightKG":1340,
    "dimensions":"250x200x150"
}
Trailer2 = {
    "name":"MiniPutt",
    "weightKG":350,
    "dimensions":"150x100x75"
}
Trailer3 = {
    "name":"Mid Differential",
    "weightKG":500,
    "dimensions":"200x150x125"
}
Trailer1Remade = {
    "name":"Bigger Bertha",
    "weightKG":2000,
    "dimensions":"250x200x150"
}

Booking1 = {
    "trailerID": 1,
    "bookerFullName": "Simon Mortensen",
    "bookerLicencePlate": "AP15244"
}
Booking2 = {
    "trailerID": 2,
    "bookerFullName": "Susanne Eriksen",
    "bookerLicencePlate": "XQ10737"
}
Booking3 = {
    "trailerID": 3,
    "bookerFullName": "Kim Højlund",
    "bookerLicencePlate": "AP15244"
}

tobeRemadeID = ""
tobeDeletedID = ""

def test_PostBooking():
    post_booking = requests.post(apiUrl+"createBooking", json=Booking1, verify=False)
    assert post_booking.status_code == 201
    
    post_booking = requests.post(apiUrl+"createBooking", json=Booking2, verify=False)
    assert post_booking.status_code == 422
    
    post_booking = requests.post(apiUrl+"createBooking", json=Booking3, verify=False)
    assert post_booking.status_code == 201

def test_GetBooking():
    get_booking = requests.get(apiUrl+"bookings", verify=False)
    assert get_booking.status_code == 200
    
    data = get_booking.json()
    ids = data[0]
    print(ids)
    assert len(ids) >= 3

def test_PostTrailer():
    global tobeRemadeID
    global last_trailerID
    post_trailer = requests.post(apiUrl+"createTrailer", json=Trailer1, verify=False)
    assert post_trailer.status_code == 201

    post_trailer = requests.post(apiUrl+"createTrailer", json=Trailer2, verify=False)
    assert post_trailer.status_code == 201
    
    post_trailer = requests.post(apiUrl+"createTrailer", json=Trailer3, verify=False)
    assert post_trailer.status_code == 201

def test_GetTrailers():
    global tobeRemadeID
    global tobeDeletedID
    get_trailer = requests.get(apiUrl+"trailers", verify=False)
    assert get_trailer.status_code == 200
    
    trailers = get_trailer.json()

    ids = [trailer["id"] for trailer in trailers]
    assert len(ids) >= 3
    print(ids)
    tobeDeletedID = str(ids[-1])
    tobeRemadeID = str(ids[0])

def test_PutTrailer():
    put_trailer = requests.put(apiUrl+"updateTrailer/"+tobeRemadeID, json=Trailer1Remade, verify=False)
    assert put_trailer.status_code == 204

    put_trailer = requests.put(apiUrl+"updateTrailer/4", json=Trailer1Remade, verify=False)
    assert put_trailer.status_code == 404

def test_DeleteTrailer():
    delete_trailer = requests.delete(apiUrl+"deleteTrailer/"+tobeDeletedID, verify=False)
    assert delete_trailer.status_code == 204

    delete_trailer = requests.delete(apiUrl+"deleteTrailer/"+tobeDeletedID, verify=False)
    assert delete_trailer.status_code == 404

    delete_all = requests.delete(apiUrl+"deleteALLTrailers", verify=False)
    assert delete_all.status_code == 204
