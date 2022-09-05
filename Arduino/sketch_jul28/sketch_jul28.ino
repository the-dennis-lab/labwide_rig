#include <Wire.h>

#define IDENTIFY    0x10
#define SET_OUTPUTS 0x20

#define BRIDGE_ADDRESS 0x7B

byte porelays[10][4];
byte found_boards;

void setup() {
  Wire.begin(); // Initialize wire library for I2C communication
  Serial.begin(9600); // Configure serial port
}

void loop() {
  byte chaser = 0x01;

  while( chaser )
  {
    bridge_set_outputs(chaser);
    delay(300);
    chaser = chaser  << 1;
  }
  bridge_set_outputs(0x00);

}

void bridge_set_outputs(byte outputs)
{
  byte checksum = (SET_OUTPUTS + outputs) & 0xFF;

  byte data[] = { SET_OUTPUTS, outputs, checksum };
  Wire.beginTransmission(BRIDGE_ADDRESS); // transmit
  Wire.write(data, 3);
  Wire.endTransmission();    // stop transmitting
}
