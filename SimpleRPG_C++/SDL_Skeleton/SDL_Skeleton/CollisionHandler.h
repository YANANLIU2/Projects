#pragma once

#include <vector>
#include <Box2D.h>

class CollisionHandler : public b2ContactListener
{
	std::vector<b2Contact*> m_pContacts;

public:
	CollisionHandler();
	~CollisionHandler();

	void BeginContact(b2Contact* pContact) override;
	void EndContact(b2Contact* pContact) override;
	void HandleCollisions();
	void ClearContacts();
};