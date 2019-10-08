const int leftLowerSensor = A1; // Analog input Moment
const int rightLowerSensor = A2; // Analog input Inside Aft
int leftRead;
int rightRead; 
int testInt;

void setup() {
  // initialize serial communications at 9600 bps:
  Serial.begin(9600);
  testInt = 0; 
  pinMode(A1, INPUT);
  pinMode(A3, INPUT);
}

void loop() {

  leftRead = analogRead(leftLowerSensor);
  rightRead = analogRead(rightLowerSensor);
  
  Serial.print(leftRead);
  Serial.print(",");
  Serial.println(rightRead);
  
//  Serial.println(testInt); 
//  testInt++;
//  if (testInt > 32000)
//  {
//    testInt = 0; 
//  }
  delay(500);
}
