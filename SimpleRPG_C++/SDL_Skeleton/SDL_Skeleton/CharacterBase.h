#pragma once
#include <SDL_render.h>
#include <unordered_map>
#include "ObjBase.h"
#include <Box2D.h>
#include "GameObject.h"

class Tile;
class CharacterBase: public GameObject
{
protected:
	bool m_isMovingUp;
	bool m_isMovingDown;
	bool m_isMovingRight;
	bool m_isMovingLeft;

	Vector2 m_position;
	Vector2 m_speed;
	Vector2 m_size;

	float m_acceleration;
	float m_maxspeed;
	float m_hp;
	bool m_dead;

	struct Frame
	{
		SDL_Rect m_src;
	};

	struct Animation
	{
		std::vector<Frame> m_frames;
		float m_frameDuration;
		bool m_shouldLoop;
	};

	enum class EAnimType
	{
		kWalkDown,
		kWalkLeft,
		kWalkRight,
		kWalkUp,


		Num_Types
	};

	std::unordered_map<EAnimType, Animation> m_animMap;

	EMove m_direction;
	EMove m_lastdirection;
	SDL_Rect m_dest;
	float m_frameTimer;
	int m_currentFrameIndex;
	EAnimType m_currentAnim;

	void Init_animations();

	SDL_Texture * m_pTexture;
	SDL_Renderer * m_pRenderer;
	const char* m_filepath;

	Vector2 m_DeltaMovement;
	bool m_CollisionBug;

public:

	enum EState
	{
		kIdle,
		kWalk,
		kDie,

		Num_Types
	};

	enum class ECharacterImageType
	{
		kFighter01,

		Num_characters
	};


protected: 
	EState m_state;
	ECharacterImageType m_ImageType;

public:
	CharacterBase(SDL_Renderer* pRenderer, b2World* pWorld, EGameObjectType type);
	void LoadATexture(const char * filepath);
	virtual ~CharacterBase();

	virtual void Init() {};
	virtual void Draw();
	virtual void Update(float deltaSeconds) {};
	void Death() { m_hp = 0; m_state = EState::kDie; m_dead = true; }
	bool IsDead() { return m_hp <= 0; }

	void Move(float deltaSeconds);
	void MoveDir(EMove direction);
	void StateCheck();
	void OutofBox();

	void NextAnimation();
	void UpdateAnimation(float deltaSeconds);
	//void ChangeState(EState newstate);

	float GetHp() { return m_hp; }
	Vector2 GetPosition() { return m_position; }
	Vector2 GetSpeed() { return m_speed; }
	Vector2 GetSize() { return m_size; }

	void Sethp(float hp) { m_hp = hp; }

	void OnTriggerEnter(b2Fixture* pOther) override {};
	void OnCollisionEnter(b2Fixture* pOther) override {};
	void OnContactEnd(b2Fixture* pOther) override {};

	bool GetCollisionBug() { return m_CollisionBug;}
	void SetCollisionBug(bool bug) { m_CollisionBug = bug; }
};
