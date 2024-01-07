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
AudioPlaySdWav           playWav1;
// Use one of these 3 output types: Digital I2S, Digital S/PDIF, or Analog DAC
AudioOutputI2S           audioOutput;
//AudioOutputSPDIF       audioOutput;
//AudioOutputAnalog      audioOutput;
AudioConnection          patchCord1(playWav1, 0, audioOutput, 0);
AudioConnection          patchCord2(playWav1, 2, audioOutput, 1);
AudioControlSGTL5000     sgtl5000_1;
String command;
String comm;
String tststr;

// Use these with the audio adaptor board
#define SDCARD_CS_PIN    10
#define SDCARD_MOSI_PIN  7
#define SDCARD_SCK_PIN   14
void setup() {
  Serial.begin(9600);
  // Audio connections require memory to work.  For more
  // detailed information, see the MemoryAndCpuUsage example
  AudioMemory(8);
  // Comment these out if not using the audio adaptor board.
  // This may wait forever if the SDA & SCL pins lack
  // pullup resistors
  sgtl5000_1.enable();
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
  delay(500);
  // Simply wait for the file to finish playing.
  while (playWav1.isPlaying()) {
  }
}
void loop() {
  if (Serial.available()) {
    comm = String(Serial.readString());
    Serial.println(comm);
    command = comm.substring(0,int(comm.indexOf('_')));
    Serial.println(command);
    Serial.println(comm);
    tststr = comm.substring(int(comm.indexOf('_'))+1);
    float vol = tststr.toFloat();
    sgtl5000_1.volume(vol);
    //if (command.equals("1")) {
      playFile("TEST.WAV");  // filenames are always uppercase 8.3 format
    //}
  }
}