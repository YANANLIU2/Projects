// GameObject.cpp
#include "GameObject.h"
#include "../Utils/Macros.h"
#include "../Utils/Random.h"
#include "SDL.h"
#include "../World/Tile.h"
#include "../World/World.h"

//---------------------------------------------------------------------------------------------------------------------
// Update function, called every frame.
//      -deltaTimeMs:   The delta since the last frame, in milliseconds.
//      -return:        The current state of the game object.  It can either be following its path or waiting for the 
//                      next path.
//---------------------------------------------------------------------------------------------------------------------
void GameObject::Update(float deltaTimeMs)
{
    std::lock_guard<std::mutex> lock(mutex);
    if (m_state == State::kFollowingPath)
    {
        StepMovement(deltaTimeMs);
    }
}

void GameObject::SetPath(Plan& path)
{
    m_path = std::move(path);
    m_state = State::kFollowingPath;
}

//---------------------------------------------------------------------------------------------------------------------
// Returns the SDL rect for this object.
//      -return:    The SDL_Rect object.
//---------------------------------------------------------------------------------------------------------------------
SDL_Rect GameObject::GetRect() const
{
    SDL_Rect rect;
    rect.x = RoundToNearest(m_position.x - (kWidth / 2.f));
    rect.y = RoundToNearest(m_position.y - (kHeight / 2.f));
    rect.w = kWidth;
    rect.h = kHeight;
    return rect;
}

const Vec2 GameObject::GetPos() const
{
    std::lock_guard<std::mutex> lock(mutex);
    return m_position;
}

//---------------------------------------------------------------------------------------------------------------------
// Causes the object to move for this frame.
//      -deltaTimeMs:   The delta since the last frame, in milliseconds.
//---------------------------------------------------------------------------------------------------------------------
void GameObject::StepMovement(float deltaTimeMs)
{
    if (m_path.size() > 0)
    {
        // step the game object towards their goal
        Vec2 direction = m_path.back() - m_position;
        direction.Normalize();
        m_position += direction * kSpeed * (float)deltaTimeMs;

        // check to see if we've arrived
        Vec2 delta = m_path.back() - m_position;
        if (delta.Length() <= kPathingNodeTolerance)
        {
            m_path.pop_back();
        }
    }
    else
    {
        m_state = State::kWaitingForPath;
    }
}


