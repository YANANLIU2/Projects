 // ObjectManager.h
#pragma once

#include "ObjectSystemTypes.h"
#include "../Application/Constants.h"
#include "../ObjectSystem/GameObject.h"
#include "../Timing/HighPrecisionTimer.h" // timer

class World;
struct SDL_Renderer;

class World;
class ObjectManager
{
	GameObject m_gameObjects[kNumGameObjects];
	World* m_pWorld;
	HighPrecisionTimer m_timer;

public: 
	// Constructor
	ObjectManager(World* pWorld) :m_pWorld(pWorld) {}

	// Destructor
	~ObjectManager() = default;

    // Game object creation & destruction
	void InitGameObjects();

    // Update
    void UpdateGameObjects(float deltaTimeMs);
    void DrawGameObjects(SDL_Renderer* pRenderer);
};
