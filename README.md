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

# Arduino Code 
- Here is sample code. According to operation and circuit diagram, plz update.

#include <LiquidCrystal_I2C.h>
#include <Wire.h>

LiquidCrystal_I2C lcd(0x27, 16, 2);
float temperature;
int reading;
int lm35Pin = A0;
int trigPin = 5;  // 초음파센서 Trig 핀
int echoPin = 4;  // 초음파센서 Echo 핀
float previousTemp = 0.0;

unsigned long previousDisplayTime = 0;
const unsigned long displayInterval = 3000;  // LCD 갱신 간격: 200ms

void setup()
{
  Serial.begin(9600);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);

  lcd.init();          // LCD 초기 설정
  delay(1000);         // LCD 초기화를 위한 1초 대기
  lcd.setBacklight(HIGH);  // LCD 백라이트 켜기
  lcd.clear();         // LCD 초기화
}

void loop()
{
  unsigned long currentMillis = millis();

  // 거리 측정
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  long duration = pulseIn(echoPin, HIGH);
  float distance = duration * 0.034 / 2;

  // 온도 측정
  int tempReading = analogRead(lm35Pin);
  float temp = (tempReading * 5.0 * 100.0) / 1024;
  float tempDiff = temp - previousTemp;  // 현재 온도와 이전 온도 값의 차이 계산
  
  // 차이가 3도 이상인 경우 시리얼 포트에 메시지 출력
  if (tempDiff >= 1.0) {
    Serial.println("high temp");
  } else if (tempDiff <= -1.0) {
    Serial.println("low temp");
  }

  // 초음파 거리가 10 이내인 경우 "warning" 출력
  if (distance <= 10.0) {
    Serial.println("warning");
  }

  // 이전 온도 값을 갱신
  previousTemp = temp;

  // 온도 및 시리얼 포트 출력
  //Serial.print("TempDiff: ");
  //Serial.println(tempDiff);

  delay(3000);  // 3초마다 측정

  // LCD에 데이터 표시
    
  if (currentMillis - previousDisplayTime >= displayInterval) {
    previousDisplayTime = currentMillis;
    lcd.setCursor(0, 0);
    lcd.print("Distance: ");
    lcd.print(distance);

    lcd.setCursor(0, 1);
    lcd.print("TempC: ");
    lcd.print(temp);

    //Serial.print("Distance: ");
    //Serial.println(distance);
    //Serial.print("Temp: ");
    //Serial.println(temp);
    if (currentMillis - previousDisplayTime >= displayInterval + 5000) {
      lcd.clear();
    }
  }
  // Serial.available() 함수를 사용하여 시리얼 통신으로 데이터를 받으면 LCD에 데이터를 표시
  if (Serial.available() > 0) {
    String inputString = "";  // 입력된 문자열을 저장할 변수

    char val = Serial.read();
    inputString += val;  // 입력된 문자를 inputString에 추가

    lcd.clear();
    lcd.setCursor(0, 0);
    lcd.print(inputString);

    // 변경된 부분: 입력된 텍스트를 바로 표시한 후에도 계속해서 추가 텍스트를 입력받고 표시합니다.
    while (Serial.available() > 0) {
      char val = Serial.read();
      inputString += val;
      lcd.print(val);
    }
    delay(3000);
  }
}
