#pragma once
#include <iostream>

#include <SDL.h>
#include <SDL_ttf.h>
#include <time.h>
#include <SDL_image.h>
#include <Box2D.h>

#include "GameStateMachine.h"
#include "SoundManager.h"
#include "MenuState.h"
#include "Tile.h"
#include "CollisionHandler.h"

using std::cout;

void AudioSetup();
bool ImageSetup(SDL_Renderer* pRenderer);

int main(int argc, char** argv)
{
	srand((unsigned int)time(NULL));

	//window initialization
	SDL_Window* pWindow = SDL_CreateWindow(GameName, WindowsFont_Width, WindiowsFont_Height, WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WINDOW_SHOWN);
	SDL_CHECK(pWindow);

	SDL_Renderer *pRenderer = SDL_CreateRenderer(pWindow, -1, SDL_RendererFlags::SDL_RENDERER_PRESENTVSYNC |
		SDL_RendererFlags::SDL_RENDERER_ACCELERATED);
	SDL_CHECK(pRenderer);

	// Audio setup
	AudioSetup();

	//deltaseconds 
	Uint64 frameCounter = SDL_GetPerformanceCounter();
	Uint64 lastFrameCounter = 0;  // Store what the counter was last frame
	double deltaSeconds = 0;

	//ttf setup
	//font initialization
	bool ttfInitialized = (TTF_Init() == 0);
	if (!ttfInitialized)
		std::cout << "Error loading SDL_ttf: " << '\n';

	//GameStateMachine initializaion
	GameStateMachine pGameStateMachine (pRenderer);
	pGameStateMachine.ChangeState(new MenuState(pRenderer));

	//image setup
	ImageSetup(pRenderer);

	//main loop
	//Image Setup
	while (pGameStateMachine.IsPlay()&&pWindow && pRenderer)
	{
		// Get the counter
		lastFrameCounter = frameCounter;
		frameCounter = SDL_GetPerformanceCounter();
		// Calculate delta time  ( * 1000 ) converts to milliseconds  ( *.001 ) Convert to seconds
		deltaSeconds = ((double)(frameCounter - lastFrameCounter) * 1000.0 / (double)SDL_GetPerformanceFrequency()) * 0.001;

		// Handle Input
		SDL_Event event;
		while (SDL_PollEvent(&event))
		{
			pGameStateMachine.GetInput(event);
		}

		//Update
		pGameStateMachine.Update(float(deltaSeconds));

		// Clear the Screen
		SDL_SetRenderDrawColor(pRenderer, 255, 255, 255, 255);
		SDL_RenderClear(pRenderer);
		//render
		pGameStateMachine.Draw();
		SDL_RenderPresent(pRenderer);

	}

	SoundManager::Get().FreeResources();
	TTF_Quit();
	IMG_Quit();
	Mix_CloseAudio();
	Mix_Quit();

	SDL_DestroyRenderer(pRenderer);
	// DestroyWindow
	SDL_DestroyWindow(pWindow);
	// Quit
	SDL_Quit();
	SDL_DestroyWindow(pWindow);

	return 0;
}

void AudioSetup()
{
	bool audioInitialized = true;
	if (Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 4096) == -1)
	{
		std::cout << "Failed to Open Audio: " << Mix_GetError() << "\n";
		audioInitialized = false;
	}

	int soundFileTypes = MIX_INIT_MP3 | MIX_INIT_OGG;
	if (Mix_Init(soundFileTypes) != soundFileTypes)
	{
		std::cout << "Failed to init sound file types...\n";
		audioInitialized = false;
	}

	SoundManager::Get().LoadAll();
}

bool ImageSetup(SDL_Renderer* pRenderer)
{
	int imgFlags = IMG_INIT_PNG | IMG_INIT_JPG;
	bool imgInitialized = (IMG_Init(imgFlags) == imgFlags);
	if (!imgInitialized)
		std::cout << "Error loading SDL_image: " << SDL_GetError() << '\n';
	// Enable alpha blending
	SDL_SetRenderDrawBlendMode(pRenderer, SDL_BlendMode::SDL_BLENDMODE_BLEND);

	Tile::TileTextureSetup(pRenderer);
	return imgInitialized;
}

