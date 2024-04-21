namespace GameObjects {
 public class Time {
  static DateTime now = DateTime.Now;
 
  private float deltaFrame = 0.0f;
  private int fps = 0;

  private int lastSecond = -1;
  private int lastMillisecond = -1;

  public Time() {}

  public int FPS => fps;
  public float DeltaFrame => deltaFrame;

  public void Update() {
   if (lastSecond == now.Second) {
    if (lastMillisecond != now.Millisecond) {
     if (lastMillisecond == -1) lastMillisecond = now.Millisecond;
     else {
      deltaFrame = ((float) now.Millisecond - lastMillisecond) / 1000;
     }
    }
    deltaFrame = 
    fps++;
   }
   else lastSecond = now.Second;  
  }
 }
}