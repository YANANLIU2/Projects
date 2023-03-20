// Main.cpp
//#include <vld.h>
#include "Application.h"
#include "SDL.h"  // must be included here or SDL will complain about main() not being defined

//---------------------------------------------------------------------------------------------------------------------
//GeneratePath optimization
//Setting: x86 release                                                HotPath
//                                                      World::GeneratePathToBestTarget 33%
//---------------------------------------------------------------------------------------------------------------------

int main(int argc, char* args[])
{
    Application app;
    if (!app.Init())
    {
        return -1;
    }
    app.MainLoop();
    return 0;
}

