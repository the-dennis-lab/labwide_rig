// wav file played on trigger from pin
// built from teensy example  http://www.pjrc.com/teensy/td_libs_AudioDataFiles.html
// edited by Nishan Shettigar
// further edited for trigger by Emily Jane Dennis
// with inspo from https://gist.github.com/stonehippo/308a5f5c49d4981ac976 
// May 2022


#include <Audio.h>
#include <Wire.h>
#include <SPI.h>
#include <SD.h>
#include <SerialFlash.h>


#define PIN 4
#define LED 13 // Most Arduinos have an LED on pin 13
#define BUTTON 2 // use pin input as a button MAY NEED TO CHANGE

AudioPlaySdWav           playWav1;
// Use one of these 3 output types: Digital I2S, Digital S/PDIF, or Analog DAC
AudioOutputI2S           audioOutput;
//AudioOutputSPDIF       audioOutput;
//AudioOutputAnalog      audioOutput;
AudioConnection          patchCord1(playWav1, 0, audioOutput, 0);
AudioConnection          patchCord2(playWav1, 1, audioOutput, 1);
AudioControlSGTL5000     sgtl5000_1;

// Use these with the audio adaptor board
#define SDCARD_CS_PIN    10
#define SDCARD_MOSI_PIN  7
#define SDCARD_SCK_PIN   14



void setup() {
  pinMode(LED, OUTPUT); // Make the LED pin active
  pinMode(BUTTON, INPUT);
  setupSound(PIN);

    Serial.begin(9600);

  // Audio connections require memory to work.  For more
  // detailed information, see the MemoryAndCpuUsage example
  AudioMemory(8);

  // Comment these out if not using the audio adaptor board.
  // This may wait forever if the SDA & SCL pins lack
  // pullup resistors
  sgtl5000_1.enable();
  sgtl5000_1.volume(0.7);

  SPI.setMOSI(SDCARD_MOSI_PIN);
  SPI.setSCK(SDCARD_SCK_PIN);
  if (!(SD.begin(SDCARD_CS_PIN))) {
    // stop here, but print a message repetitively
    while (1) {
      Serial.println("Unable to access the SD card");
      delay(500);
    }
  }
}





void playFile(const char *filename)
{
  Serial.print("Playing file: ");
  Serial.println(filename);

  // Start playing the file.  This sketch continues to
  // run while the file plays.
  playWav1.play(filename);

  // A brief delay for the library read WAV info
  delay(5);

  // Simply wait for the file to finish playing.
  while (playWav1.isPlaying()) {
    // uncomment these lines if you audio shield
    // has the optional volume pot soldered
    //float vol = analogRead(15);
    //vol = vol / 1024;
    // sgtl5000_1.volume(vol);
  }
}

void loop() {
  if (digitalRead(BUTTON) == HIGH) {
    activateSound(PIN);
  }
}

void setupSound(int pin) {
  pinMode(pin, OUTPUT);
  digitalWrite(pin, LOW); // Set the pin low as the default state
}

void activateSound(int pin) {
  digitalWrite(pin, HIGH); // bring the pin low to begin the activation
  /*
  According to the documentation, the Audio FX board needs 50ms to trigger. However,
  I've found that coming from my 3.3v Arduino Pro, it needs 100ms to get the trigger
  the going
  */
  delay(50); // hold the pin low long enough to trigger the board; may need to be longer for consistent triggering
  digitalWrite(pin, LOW); // bring the pin high again to end the activation
}
