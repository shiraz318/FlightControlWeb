 # FlightControlWeb
 ## Overview
 In this project we created an aerial control system who monitors active flights and is able to accept new flight plans.  
The flight plan define the path of the flight and other technical details.  
The system is syncronized with other aerial control systems, such that each user can keep track of external flights as well as the internal flights.  

## How to use
1. Download the FlightControl folder, and put it inside a zip folder called "FlightControl".  
2. Locate the build and run file in the same location as the zip folder, and double click the build and run file.  
3. You should see now a black window with url address.  
4. Copy that address to a chrome browser, and the web will open up.  

      ### Internal Flights
      If you wish to upload new internal flights, you can use the example of a jsno flightplan "example_flight_plan" and write your own details.  
      After you updated the json file, drag and drop it in the internal flights area, and you can click on it and see it's path on the map and the details of flight plan below.  

      ### External Flights
      If you wish to see external flights from different aerial control system, you need to use "Postmen" app, with (url from the black window)/api/servers, and in the body fill the details in this patterns {"ServerId": "(id)","ServerURL": "(other system url)"} in json form.  
      The external flights will appear in the external flights section.  


### Note
In order to activate tha map, you need to write a key in wwwroot/index.html instead of (Enter_Your_Key_Here).  
To get a key, use this link https://developers.google.com/maps/documentation/javascript/get-api-key
and follow the instuctions.


#### Versioning  
We used [Github](https://github.com/shiraz318/FlightControlWeb) for version control

#### Authors
Nili Cohen and Shiraz Berger.
