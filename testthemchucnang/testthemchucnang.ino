#include <RH_NRF24.h>
#include <EEPROM.h>
#include <ESP8266WiFi.h>
#include <NTPClient.h>
#include <WiFiUdp.h>
#include "FirebaseESP8266.h"
#define TOKEN  "BBFF-g8WpqytcXSWGHd0ArLxjFZkY7Wtgsa"  // Put here your Ubidots TOKEN

#define FIREBASE_HOST "https://esp8266project-8ea11-default-rtdb.firebaseio.com/"
#define FIREBASE_AUTH "8TeUixsbsKGGZJ6lVUh6K8wXG6DpZl4WOyCjl0Mj"

unsigned long lastMillis = 0;

String apiKey = "H2CUSMRLP2SSNAZS";

const char* ssid = "Setup";
const char* password = "midaspro";
 
WiFiClient client;
int gatewayID = EEPROM.read(0);

const char* server = "api.thingspeak.com";
 
RH_NRF24 nrf24(2, 4); 


// Define NTP Client to get time
WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP);

//Define FirebaseESP8266 data object
FirebaseData firebaseData;
FirebaseData firebaseData1;
FirebaseData firebaseData2;
FirebaseData realtimeData;
FirebaseData ledData;
FirebaseJson json;
const int ledPin = 4; //GPIO4 or D2 for LED
const int swPin = 5;  //GPIO5 or D1 for Switch
bool swState = false;
String path = "/Nodes";
String nodeID = "Node2";      //This is this node ID to receive control
String otherNodeID = "Node1"; //This is other node ID to control


unsigned long sendDataPrevMillis = 0;
// Variables to save date and time
String formattedDate;
String dayStamp;
String timeStamp;

float devices;

void setup()
{
  Serial.begin(115200);
  
  pinMode(ledPin, OUTPUT);
  pinMode(swPin, INPUT);
  
  Serial.print("Receiver Started, ID: ");
  Serial.print("Connecting to ");
  Serial.println(ssid);
  Serial.print(gatewayID);
  Serial.println();
  
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) 
  {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  
  nrf24.init();
  nrf24.setChannel(3);
  nrf24.setRF(RH_NRF24::DataRate2Mbps, RH_NRF24::TransmitPower0dBm);
  
  // Initialize a NTPClient to get time
  timeClient.begin();
  timeClient.setTimeOffset(+7*60*60);
  
  // Initialize Firebase
  Firebase.begin(FIREBASE_HOST, FIREBASE_AUTH);
  Firebase.reconnectWiFi(true);

  if (!Firebase.beginStream(firebaseData1, path + "/" + nodeID))
    {
        Serial.println("Could not begin stream");
        Serial.println("REASON: " + firebaseData1.errorReason());
        Serial.println();
    }
}

void timeUpdate(){
  while(!timeClient.update()) {
    timeClient.forceUpdate();
  }
  // The formattedDate comes with the following format:
  // 2018-05-28T16:00:13Z
  // We need to extract date and time
  formattedDate = timeClient.getFormattedDate();
  Serial.println("Date-Time");
  Serial.println(formattedDate);
  // Extract date
  int splitT = formattedDate.indexOf("T");
  dayStamp = formattedDate.substring(0, splitT);
  json.add("Day",dayStamp);
  Serial.println(dayStamp);
  timeStamp = formattedDate.substring(splitT+1, formattedDate.length()-1);
  json.add("Time",timeStamp);
  Serial.println(timeStamp);
}

void loop()
{
  if (nrf24.available())
  {
    // Should be a message for us now
    uint8_t buf[RH_NRF24_MAX_MESSAGE_LEN];
    uint8_t len = sizeof(buf);
    if (nrf24.recv(buf, &len))
    {
 
      // Send a reply
      uint8_t sdata[] = "Data Received.";
      nrf24.send(sdata, sizeof(sdata));
      nrf24.waitPacketSent();

      timeUpdate();
      String stat = "Active";
      float humidity = buf[0];
      float temperature = buf[1];
      int deviceID = buf[2];
      json.add("Temperature",temperature); //add (new) data
      json.add("Humidity",humidity);
      json.add("Status",stat);

      
 
      Serial.println("--- Data retrieved from device ---");

        if (client.connect(server, 80)) { // "184.106.153.149" or api.thingspeak.com
        String postStr = apiKey;
        postStr += "&field1=";
        postStr += String(temperature);
        postStr += "&field2=";
        postStr += String(humidity);
        postStr += "&field3=";
        postStr += String(gatewayID);
        postStr += "&field4=";
        postStr += String(deviceID);
        postStr += "\r\n\r\n";
        
        client.print("POST /update HTTP/1.1\n");
        client.print("Host: api.thingspeak.com\n");
        client.print("Connection: close\n");
        client.print("X-THINGSPEAKAPIKEY: " + apiKey + "\n");
        client.print("Content-Type: application/x-www-form-urlencoded\n");
        client.print("Content-Length: ");
        client.print(postStr.length());
        client.print("\n\n");
        client.print(postStr);
        
        Serial.println("---- Data sent to Thingspeak and Firebase ----");
        Serial.print("Device ID: ");
        Serial.print(deviceID);
        Serial.print(", Temperature:");
        Serial.print(temperature);
        Serial.print(", Humidity:");
        Serial.println(humidity);
      }

      {

      Firebase.pushJSON(firebaseData, "FirebaseIOT", json);
      Firebase.setFloat(firebaseData, "RealtimeData/Temperature",temperature);
      Firebase.setFloat(firebaseData, "RealtimeData/Humidity",humidity);
      }
    }
   
  else 
  {
 
    //Serial.print("No New Message");
    
  }
  delay(3000);
  
  if (!Firebase.readStream(firebaseData1))
    {
        Serial.println();
        Serial.println("Can't read stream data");
        Serial.println("REASON: " + firebaseData1.errorReason());
        Serial.println();
    }

    if (firebaseData1.streamTimeout())
    {
        Serial.println();
        Serial.println("Stream timeout, resume streaming...");
        Serial.println();
    }

    if (firebaseData1.streamAvailable())
    {
        if (firebaseData1.dataType() == "boolean")
        {
            if (firebaseData1.boolData())
                Serial.println("Set " + nodeID + " to High");
            else
                Serial.println("Set " + nodeID + " to Low");
            digitalWrite(ledPin, firebaseData1.boolData());
        }
    }
  }
}
