using KeyboardCommander.Engine;
using KeyboardCommander.States;

int Width = 1280;
int Height = 720;

using var game = new MainGame(Width, Height, new GameplayState());
game.Run();
