// Main.cpp
//#include <vld.h>
#include "Application.h"
#include "SDL.h"  // must be included here or SDL will complain about main() not being defined

//---------------------------------------------------------------------------------------------------------------------
//GeneratePath optimization
//Setting: x86 release                                                HotPath
//                                                      World::GeneratePathToBestTarget 55.91%          std::list 43.78%
//1. Make openset vector:                               World::GeneratePathToBestTarget 27.43%          World::GetNeighors 11.34%
//2. Make PlanningNodes from std::list to std::vector   World::GeneratePathToBestTarget 16.05%          World::GetNeighors 5.07%
//3. Make OutNeighbors array:                           World::GeneratePathToBestTarget 15.04%          std::sort_unchecked 1.45%
//4. Make GameObjects array:                            World::GeneratePathToBestTarget 14.33%          std::sort_unchecked 1.69%
//5. Make PlanningNodes std::vector<std::pair<...>>     World::GeneratePathToBestTarget 11.03%          std::sort_unchecked 1.03%
//6. Minor: Make unrolling loops, time slicing          World::GeneratePathToBestTarget 10.08%          std::sort_unchecked 1.27%

//(the below record w/o time slicing changes)
//7. remove plannningNodes and use priority queue       World::GeneratePathToBestTarget 0.37%
//8. find shortest path first, then random pick one of the rest goals        World::GeneratePathToBestTarget 0.59%

//9. Minor: move gameobj draw to objectmanager to save render color set up. 
//10. GuessAClosestGoal: by |x1-x2|+|y1-y2|, then pick a random goal in the rest, 
//    do dij from the start to the goal  World::GeneratePathToBestTarget 0.24%
//---------------------------------------------------------------------------------------------------------------------

int main(int argc, char* args[])
{
    Application app;

    if (!app.Init())
        return -1;

    app.MainLoop();

    return 0;
}

