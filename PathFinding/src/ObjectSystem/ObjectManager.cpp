// ObjectManager.cpp
#include "ObjectManager.h"
#include "../World/World.h"
#include "GameObject.h"
#include "SDL_rect.h"
#include "../World/Tile.h"
void ObjectManager::InitGameObjects()
{
	for (int i = 0; i < kNumGameObjects; i++)
	{
		if (m_pWorld)
		{
			if (auto tile = m_pWorld->GetTile(m_pWorld->GetRandWalkableTile()))
			{
				Vec2 tempPos = tile->GetCenter();
				m_gameObjects[i].Init(tempPos.x, tempPos.y);
			}
		}
	}
}

//---------------------------------------------------------------------------------------------------------------------
// Updates all game objects.
//      -deltaTimeMs:   The delta since the last frame, in milliseconds.
//      -pWorld:        The world the game objects live in.
//---------------------------------------------------------------------------------------------------------------------
void ObjectManager::UpdateGameObjects(float deltaTimeMs)
{
	m_timer.StartTimer();
	bool bIsDonePathFinding = false;
	for (int i = 0; i < kNumGameObjects; i++)
	{
		if (!bIsDonePathFinding)
		{
			if (!(bIsDonePathFinding = m_timer.GetTimer() > kPathFindingComputingTimePerFrame))
			{
				if (m_gameObjects[i].GetState() == GameObject::State::kWaitingForPath)
				{
					Plan path;
					m_pWorld->GeneratePathToBestTarget(m_gameObjects[i].GetPos(), path);
					if (!path.empty())
					{
						m_gameObjects[i].SetPath(path);
					}
				}
			}
		}
		else
		{
			break;
		}
	}

	for (int i = 0; i < kNumGameObjects; i++)
	{
		if (m_gameObjects[i].GetState() == GameObject::State::kFollowingPath)
		{
			m_gameObjects[i].Update(deltaTimeMs);
		}
	}
}

//---------------------------------------------------------------------------------------------------------------------
// Draws all game objects.
//      -pRenderer: The SDL renderer.
//---------------------------------------------------------------------------------------------------------------------
void ObjectManager::DrawGameObjects(SDL_Renderer* pRenderer)
{
	SDL_SetRenderDrawColor(pRenderer, 0xff, 0, 0, 0x00);
	for (int i = 0; i < kNumGameObjects; ++i)
	{
		 const SDL_Rect rect = m_gameObjects[i].GetRect();
		SDL_RenderFillRect(pRenderer, &rect);
	}
}
