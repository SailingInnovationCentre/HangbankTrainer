const int leftLowerSensor = A1; // Analog input Moment
const int rightLowerSensor = A2; // Analog input Inside Aft
int leftRead;
int rightRead; 

void setup() {
  // initialize serial communications at 9600 bps:
  Serial.begin(9600);
  pinMode(A1, INPUT);
  pinMode(A3, INPUT);
}

void loop() {

  leftRead = analogRead(leftLowerSensor);
  rightRead = analogRead(rightLowerSensor);
  
  Serial.print("  Links: ");
  Serial.print(leftRead);

  Serial.print("  Rechts: ");
  Serial.println(rightRead);

//  value = momentValue;
//  double gewicht = value * 0.3;

//  Serial.print("Gewicht: ");
//  Serial.println(gewicht);

//  Serial.print("C ");
//  rm = gewicht*0.45;
//  moment = (gewicht*0.45);
//  Serial.println(moment);
//  Serial.print("D");
//  Serial.println(moment);
//  Serial.print("E");
//  Serial.println(gewicht);
//  
//  Serial.print("F");
//  Serial.println(OutForV*cal);
//  Serial.print("G");
//  Serial.println(InForV*cal);
//  Serial.print("H");
//  Serial.println(OutAftV*cal);
//  Serial.print("I");
//  Serial.println(InAftV*cal);
//  
  delay(1000);
}
