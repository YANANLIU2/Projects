// GameObject.h
#pragma once

#include "SDL_rect.h"
#include "../World/PathPlan.h"
#include "../Utils/Vector.h"

struct SDL_Renderer;
class GameObject
{
public:
	enum class State
	{
		kWaitingForPath,
		kFollowingPath
	};

	static constexpr int kWidth = 8;
	static constexpr int kHeight = 8;
	static constexpr float kSpeed = 0.05f;
	static constexpr unsigned char kMinColorValue = 55;
	static constexpr float kPathingNodeTolerance = 4.f;

	Vec2 m_position;
	Plan m_path;
	State m_state;

	unsigned char m_red, m_green, m_blue;

public:
	GameObject()
		: m_red(0xff)
		, m_green(0)
		, m_blue(0)
		, m_position(0,0)
		, m_path()
		, m_state(State::kWaitingForPath)
	{};

	void Init(float x, float y) { m_position = { x, y }; }

    // Update functions
    void Update(float deltaTimeMs);

    // Getters & Setters
	void SetPath(Plan& path);
	void SetState(State newState) { m_state = newState; }
	SDL_Rect GetRect() const;
	inline const Vec2 GetPos() const { return m_position; }
	inline State GetState() const { return m_state; }

private:
    void StepMovement(float deltaTimeMs);
};
