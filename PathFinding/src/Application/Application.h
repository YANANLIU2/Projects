// Application.h
#pragma once

#include "../Timer/HighPrecisionTimer.h"

struct SDL_Window;
struct SDL_Renderer;

class ObjectManager;
class World;

class Application
{
    static constexpr int kWindowWidth = 960;
    static constexpr int kWindowHeight = 960;

    World* m_pWorld;
    ObjectManager* m_pObjectMgr;

    // Rendering
    SDL_Window* m_pWindow;
    SDL_Renderer* m_pRenderer;

    // frame count stuff
    HighPrecisionTimer m_frameTimer;
    double m_deltaTime;

public:
    // Constructor
    Application() 
        : m_pWindow(nullptr)
        , m_pRenderer(nullptr)
        , m_pObjectMgr(nullptr)
        , m_pWorld(nullptr)
        , m_deltaTime(0)
    {
    }

    // Destructor
    ~Application();

    // Init the world
    bool Init();

    void MainLoop();

private:
    // Polls for and process events. Returns true for exiting.
    bool ProcessEvents();  
};
