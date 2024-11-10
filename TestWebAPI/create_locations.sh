curl -X 'POST' \
  'http://0.0.0.0:5269/api/Location/load_locations' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '[
    {
      "Id": 1,
      "Name": "Central Park",
      "Address": "New York, NY 10024",
      "Capacity": 5000
    },
    {
      "Id": 2,
      "Name": "Madison Square Garden",
      "Address": "4 Pennsylvania Plaza, New York, NY 10001",
      "Capacity": 20000
    },
    {
      "Id": 3,
      "Name": "Hollywood Bowl",
      "Address": "2301 N Highland Ave, Los Angeles, CA 90068",
      "Capacity": 17500
    },
    {
      "Id": 4,
      "Name": "Red Rocks Amphitheatre",
      "Address": "18300 W Alameda Pkwy, Morrison, CO 80465",
      "Capacity": 9525
    },
    {
      "Id": 5,
      "Name": "Staples Center",
      "Address": "1111 S Figueroa St, Los Angeles, CA 90015",
      "Capacity": 21000
    },
    {
      "Id": 6,
      "Name": "Grant Park",
      "Address": "337 E Randolph St, Chicago, IL 60601",
      "Capacity": 70000
    },
    {
      "Id": 7,
      "Name": "The Ryman Auditorium",
      "Address": "116 5th Ave N, Nashville, TN 37219",
      "Capacity": 2362
    },
    {
      "Id": 8,
      "Name": "The Gorge Amphitheatre",
      "Address": "754 Silica Rd NW, George, WA 98848",
      "Capacity": 27000
    },
    {
      "Id": 9,
      "Name": "Wrigley Field",
      "Address": "1060 W Addison St, Chicago, IL 60613",
      "Capacity": 42000
    },
    {
      "Id": 10,
      "Name": "Fenway Park",
      "Address": "4 Jersey St, Boston, MA 02215",
      "Capacity": 37731
    }
  ]
  '