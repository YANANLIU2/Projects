#pragma once

//create for collision handler to tell which one is player
#include "Box2D.h"
#include "Defines.h"

class GameObject
{
protected:
	EGameObjectType m_ObjectType;
	b2Body* m_pBody;
	intpair m_WidthHeight;
	intpair m_LeftCorner;
	b2World* m_pWorld;
	bool m_hideCharacter;
	ETileType m_Tiletype;

public:
	GameObject(EGameObjectType type, intpair widthHeight, intpair leftCorner, b2World* pWorld, ETileType tiletype);
	virtual ~GameObject();
	EGameObjectType GetType() { return m_ObjectType; }

	virtual void OnTriggerEnter(b2Fixture* pOther) {};
	virtual void OnCollisionEnter(b2Fixture* pOther) {};
	virtual void OnContactEnd(b2Fixture* pPlayer) {};
	void SetCollisionBody();
	bool GetHide() { return m_hideCharacter; }
	virtual void Draw() {};

	intpair GetLeftCorner() { return m_LeftCorner; }
	intpair GetWidthHeight() { return { m_WidthHeight.x*k_tilesize, m_WidthHeight.y*k_tilesize }; }
};



