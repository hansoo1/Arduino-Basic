<img src="https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=header&text=Hansoo&fontSize=90">
# Arduino-Basic with UNITY

Simple example made by Arduino & UNITY 

# Practice : Making a robot that can surprise parents in daily life

# Step1. Conceptual design

1. Basic Function
- Surprise event for parents

2. Main Requirement
- Confess that you love your parents often
- gift a smile
- Recognize the parents' basic situation

3. How to design
- make to think of famliy (like a child's face)
- Use 3D printer

# Step2. Working Princple

- Sub Function 1) The design which reminds you of family (made by 3D Printer)
- Sub Function 2) One-way communication using LCD module
- Sub Function 3) motion detection using ultrasonic sensor
- Sub Function 4) Check the condition of the house using a temperature sensor

# Step3. Make Arduino_circuit
![image](https://github.com/hansoo1/Arduino-Basic-with-UNITY/assets/107674388/d7c114e2-495d-4e60-a727-4d4e1710c62a)

#Components
- Arduino (ATMEGA32P)
- HC-SR04 (ultrasonic sensor)
- LM35DZ (temperature sensor)
- LCD 16x2 4pin(I2C control)


# Arduino Code 
- Here is sample code. According to operation and circuit diagram, plz update.

#include <LiquidCrystal_I2C.h>
#include <Wire.h>

  LiquidCrystal_I2C lcd(0x27, 16, 2);
  float temperature;
  int reading;
  int lm35Pin = A0;
  int trigPin = 5;
  int echoPin = 4;
  float previousTemp = 0.0;

  unsigned long previousDisplayTime = 0;
  const unsigned long displayInterval = 3000;

  void setup(){
  Serial.begin(9600);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);

  lcd.init();          
  delay(1000);         
  lcd.setBacklight(HIGH); 
  lcd.clear();        
  }

  void loop()
  {
  unsigned long currentMillis = millis();
  
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  long duration = pulseIn(echoPin, HIGH);
  float distance = duration * 0.034 / 2;
  int tempReading = analogRead(lm35Pin);
  float temp = (tempReading * 5.0 * 100.0) / 1024;
  float tempDiff = temp - previousTemp;  
  

  if (tempDiff >= 1.0) {
    Serial.println("high temp");
  } else if (tempDiff <= -1.0) {
    Serial.println("low temp");
  }

 
  if (distance <= 10.0) {
    Serial.println("warning");
  }
  previousTemp = temp;  
  Serial.print("TempDiff: ");
  Serial.println(tempDiff);
  delay(3000); 
    
  if (currentMillis - previousDisplayTime >= displayInterval) {
    previousDisplayTime = currentMillis;
    lcd.setCursor(0, 0);
    lcd.print("Distance: ");
    lcd.print(distance);

    lcd.setCursor(0, 1);
    lcd.print("TempC: ");
    lcd.print(temp);

    Serial.print("Distance: ");
    Serial.println(distance);
    Serial.print("Temp: ");
    Serial.println(temp);
    if (currentMillis - previousDisplayTime >= displayInterval + 5000) {
      lcd.clear();
    }
  }
  
  if (Serial.available() > 0) {
    String inputString = "";  
    char val = Serial.read();
    inputString += val;  
    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print(inputString);
    
    while (Serial.available() > 0) {
      char val = Serial.read();
      inputString += val;
      lcd.print(val);
    }
    delay(3000);
  }
}
