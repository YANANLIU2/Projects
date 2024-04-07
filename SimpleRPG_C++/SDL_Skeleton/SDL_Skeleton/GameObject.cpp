#include "GameObject.h"


GameObject::GameObject(EGameObjectType type, intpair widthHeight, intpair leftCorner, b2World * pWorld, ETileType tiletype)
{
	m_ObjectType = type;
	m_WidthHeight = widthHeight;
	m_LeftCorner = leftCorner;
	m_pWorld = pWorld;
	m_hideCharacter = false;
	m_Tiletype = tiletype;

	SetCollisionBody();
}

GameObject::~GameObject()
{
	m_pBody->DestroyFixture(m_pBody->GetFixtureList());
}

void GameObject::SetCollisionBody()
{
	if (m_pBody)
	{
		m_pBody->DestroyFixture(m_pBody->GetFixtureList());
		m_pBody = nullptr;
	}

	b2BodyDef bodyDef;
	bodyDef.type = b2_staticBody;

	switch (m_ObjectType)
	{
	case EGameObjectType::kBlock:
	case EGameObjectType::kTile:
		bodyDef.type = b2_kinematicBody;
		break;
	case EGameObjectType::kPlayer:
		bodyDef.type = b2_dynamicBody;

	default:
		break;
	}

	//center point
	float halfwidth = m_WidthHeight.x*k_tilesize / 2.f / kPixelsPerMeter;
	float halfheight = m_WidthHeight.y*k_tilesize / 2.f / kPixelsPerMeter;
	float centre_x = float(m_LeftCorner.x) / kPixelsPerMeter + halfwidth;
	float centre_y = float(m_LeftCorner.y) / kPixelsPerMeter + halfheight;

	bodyDef.position.Set(centre_x, centre_y);

	// Use definition to add a body to our world
	m_pBody = m_pWorld->CreateBody(&bodyDef);
	m_pBody->SetUserData(this);

	// Create a shape
	b2PolygonShape shape;
	shape.SetAsBox(halfwidth, halfheight);

	// Create the fixture definition
	b2FixtureDef fixtureDef;
	if (m_Tiletype == ETileType::kTree)
		fixtureDef.isSensor = true;

	fixtureDef.shape = &shape;

	// Add the shaped fixture to the body
	m_pBody->CreateFixture(&fixtureDef);
}


