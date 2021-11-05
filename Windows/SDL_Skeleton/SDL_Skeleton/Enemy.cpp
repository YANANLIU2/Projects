#include "Enemy.h"
#include <iostream>

//Enemy::Enemy(SDL_Renderer* pRenderer, ECharacterType type, EMoveway moveway) :CharacterBase(pRenderer)
//{
//	m_moveway = moveway;
//
//	switch (m_moveway)
//	{
//	case EMoveway::kMoveAwayPlayer:
//		MoveAwayPlayer();
//		break;
//	case EMoveway::kMoveTowardsPlayer:
//		MoveTowardsPlayer();
//		break;
//	case EMoveway::kMoveRandomly:
//		MoveRandomly();
//		break;
//	default:
//		std::cout << "Invalid moveway.\n";
//		break;
//	}
//
//	m_type = type;
//	this->SetFilepath();
//}
//
//
//Enemy::Enemy(SDL_Renderer * pRenderer) : CharacterBase(pRenderer)
//{
//}
//
//Enemy::~Enemy()
//{
//}
//
//void Enemy::Init()
//{
//	m_position.x = float(rand()%WINDOW_WIDTH + 1);
//	m_position.y = float(rand() % WINDOW_HEIGHT + 1);
//	m_dest = { int(m_position.x),int(m_position.y) ,128,128 };
//	m_speed.x = 0;
//	m_speed.y = 0;
//	m_acceleration = float(0.02);
//	m_maxspeed = float(rand() % 5 / 10.0 + 0.1);
//	//m_color_r = m_color_g = m_color_b = m_color_a = 0;
//	m_size.x = 80;
//	m_size.y = 100;
//	m_hp = 100;
//
//}
//
//void Enemy::Update(float deltaSeconds)
//{
//	if (!this->IsDead())
//	{
//		OutofBox();
//		m_player.x = playerPosition.x;
//		m_player.y = playerPosition.y;
//		MoveTowardsPlayer();
//		IdleStateCheck();
//		Move(deltaSeconds);
//		m_dest = { int(m_position.x), int(m_position.y), int(m_size.x), int(m_size.y) };
//
//		UpdateAnimation(deltaSeconds);
//	}
//
//
//	
//}
//
//void Enemy::MoveTowardsPlayer()
//{
//	m_isMovingLeft = m_isMovingRight = m_isMovingUp = m_isMovingDown = false;
//
//	if (m_position.y > m_playerPosition.y)
//		MoveDir(EMove::Up);
//	else if (m_position.y < m_playerPosition.y)
//		MoveDir(EMove::Down);
//
//	if (m_position.x > m_playerPosition.x)
//	{
//		MoveDir(EMove::Left);
//		FlipTexture();
//	}
//	else if (m_position.x < m_playerPosition.x)
//		MoveDir(EMove::Right);
//
//}
//
//void Enemy::MoveAwayPlayer()
//{
//	m_isMovingUp = false;
//	m_isMovingDown = false;
//	m_isMovingRight = false;
//	m_isMovingLeft = false;
//
//	if (m_position.x > m_playerPosition.x)
//		MoveDir(EMove::Right);
//	else if (m_position.x < m_playerPosition.x)
//		MoveDir(EMove::Left);
//
//	if (m_position.y > m_playerPosition.y)
//		MoveDir(EMove::Down);
//
//	else if (m_position.y < m_playerPosition.y)
//		MoveDir(EMove::Up);
//
//}
//
//void Enemy::MoveRandomly()
//{
//	m_isMovingDown = false;
//	m_isMovingDown = false;
//	m_isMovingRight = false;
//	m_isMovingUp = false;
//
//	int randnum = rand() % 5;
//	switch (randnum)
//	{
//	case 0: 
//		break;
//	case 1:
//		m_isMovingDown = true;
//		break;
//	case 2:
//		m_isMovingDown = true;
//		break;
//	case 3:
//		m_isMovingRight = true;
//		break;
//	case 4:
//		m_isMovingUp = true;
//		break;
//	default:
//		std::cout << "Invalid rand direction.\n";
//		break;
//	
//
//	}
//}
//
//Enemy * Enemy::MakeClericEnemy(SDL_Renderer* pRenderer)
//{
//	return new Enemy(pRenderer, ECharacterType::kCleric, EMoveway::kMoveTowardsPlayer);
//}
//
//Enemy * Enemy::MakeWarriorEnemy(SDL_Renderer* pRenderer)
//{
//	return new Enemy(pRenderer, ECharacterType::kWarrior, EMoveway::kMoveTowardsPlayer);
//
//}
//
//Enemy * Enemy::MakeOrcEnemy(SDL_Renderer* pRenderer)
//{
//	return new Enemy(pRenderer, ECharacterType::kOrc, EMoveway::kMoveTowardsPlayer);
//}
//
//Enemy * Enemy::MakeRangerEnenmy(SDL_Renderer* pRenderer)
//{
//	return new Enemy(pRenderer, ECharacterType::kRanger, EMoveway::kMoveTowardsPlayer);
//}
//

