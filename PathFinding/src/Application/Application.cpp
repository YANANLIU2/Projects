// Application.cpp
#include "Application.h"
#include "SDL.h"
#include "../Utils/Macros.h"
#include "../Utils/Random.h"
#include "../ObjectSystem/ObjectManager.h"
#include "../World/World.h"
#include "../World/Tile.h"
#include <assert.h>
#include <stdlib.h>
#include <time.h>
#include <sstream>

//---------------------------------------------------------------------------------------------------------------------
// Destructor.
//---------------------------------------------------------------------------------------------------------------------
Application::~Application()
{
    SAFE_DELETE(m_pObjectMgr);
    SAFE_DELETE(m_pWorld);

    SDL_DestroyRenderer(m_pRenderer);
    SDL_DestroyWindow(m_pWindow);

    SDL_Quit();
}

//---------------------------------------------------------------------------------------------------------------------
// Initializes the application.  This will set up all the SDL stuff as well as the world.  Returns true if it succeeds
// and false if it fails.  If it fails, it will also log an error.
//---------------------------------------------------------------------------------------------------------------------
bool Application::Init()
{
    // Initialize the rng
    Random::Seed();

    // Attempt to initialize SDL
    if (SDL_Init(SDL_INIT_VIDEO) < 0)
    {
        SDL_LogError(SDL_LOG_CATEGORY_APPLICATION, "SDL could not initialize! SDL_Error: %s", SDL_GetError());
        return false;
    }

    // Create the window and renderer
    if (SDL_CreateWindowAndRenderer(kWindowWidth, kWindowHeight, SDL_WINDOW_SHOWN, &m_pWindow, &m_pRenderer))
    {
        SDL_LogError(SDL_LOG_CATEGORY_APPLICATION, "Couldn't create window and renderer: %s", SDL_GetError());
        return false;
    }

    // Set the title
    SDL_SetWindowTitle(m_pWindow, "Path Finding");

    // Create the world
    m_pWorld = new World;
    if (!m_pWorld->Init("../bin/data/map.txt"))
    {
        SDL_LogError(SDL_LOG_CATEGORY_APPLICATION, "Failed to create world: %s.", SDL_GetError());
        return false;
    }

	// Create the object manager
	m_pObjectMgr = new ObjectManager(m_pWorld);

    // Spawn agents
	m_pObjectMgr->InitGameObjects();

    return true;
}

//---------------------------------------------------------------------------------------------------------------------
// Executes the main loop.  This function won't exit until the game is done.
//---------------------------------------------------------------------------------------------------------------------
void Application::MainLoop()
{
    while (true)
    {
        m_frameTimer.StartTimer();

        // Process all the events
        if (ProcessEvents())
        {
            break;
        }

        // Update the agents
        m_pObjectMgr->UpdateGameObjects(float(m_deltaTime));

        // Clear the screen.
        SDL_SetRenderDrawColor(m_pRenderer, 0xff, 0xff, 0xff, 0x00);
        SDL_RenderClear(m_pRenderer);

        // Render the world
        m_pWorld->Render(m_pRenderer);

        // Render the agents
        m_pObjectMgr->DrawGameObjects(m_pRenderer);

        // Present the renderer
        SDL_RenderPresent(m_pRenderer);

        m_deltaTime = m_frameTimer.GetTimer();

#ifdef _DEBUG // For breakpoints
        if (m_deltaTime >= 500.0)
        {
            m_deltaTime = 16.67;
        }
#endif
    }
}

//---------------------------------------------------------------------------------------------------------------------
// Polls for events and processes the ones we care about.  Returns true if we want to exit the program, false if not.
//---------------------------------------------------------------------------------------------------------------------
bool Application::ProcessEvents()
{
    SDL_Event event;

    // Poll for an event
    SDL_PollEvent(&event);

    // Process the event
    switch (event.type)
    {
        case SDL_KEYDOWN:
            if (event.key.keysym.sym == SDLK_ESCAPE)
            {
                return true;
            }
        break;

        case SDL_QUIT:
            return true;
        default:
            return false;
    }

    return false;
}
