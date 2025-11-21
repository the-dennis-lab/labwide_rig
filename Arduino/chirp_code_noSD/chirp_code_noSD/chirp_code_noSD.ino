// wav file played on trigger from onboard memory
// built from teensy example  http://www.pjrc.com/teensy/td_libs_AudioDataFiles.html
// edited by Nishan Shettigar
// further edited for trigger, not using SD by Emily Jane Dennis
// with inspo from https://gist.github.com/stonehippo/308a5f5c49d4981ac976
// May 2022
#include <Audio.h>
#include <Wire.h>
#include <SPI.h>

#include "AudioSampleTest.h" 

AudioPlayMemory          sound0;
//AudioPlaySdWav           playWav1;
// Use one of these 3 output types: Digital I2S, Digital S/PDIF, or Analog DAC
AudioOutputI2S           audioOutput;
//AudioOutputSPDIF       audioOutput;
//AudioOutputAnalog      audioOutput;
AudioConnection          patchCord1(sound0, 0, audioOutput, 0);
AudioConnection          patchCord2(sound0, 2, audioOutput, 1);
AudioControlSGTL5000     sgtl5000_1;
String command;
String comm;
String commString;
// Use these with the audio adaptor board

void setup() {
  Serial.begin(9600);
  // Audio connections require memory to work.  For more
  // detailed information, see the MemoryAndCpuUsage example
  AudioMemory(8);
  sgtl5000_1.enable();
}


void loop() {
  if (Serial.available()) {
    Serial.setTimeout(50);
    comm = String(Serial.readString());
    Serial.println(comm);
    command = comm.substring(0, int(comm.indexOf('_')));
    Serial.println(command);
    Serial.println(comm);
    commString = comm.substring(int(comm.indexOf('_')) + 1);
    float vol = commString.toFloat();
    sgtl5000_1.volume(vol);
    if (command.equals("1")) {
      sound0.play(AudioSampleTest);  
    }
  }
}
