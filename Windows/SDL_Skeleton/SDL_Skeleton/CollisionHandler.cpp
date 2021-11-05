#include "CollisionHandler.h"

#include <iostream>

#include "GameState.h"
#include "GameObject.h"
#include "CharacterBase.h"

CollisionHandler::CollisionHandler()
{
}


CollisionHandler::~CollisionHandler()
{
}

void CollisionHandler::BeginContact(b2Contact* pContact)
{
	m_pContacts.push_back(pContact);
}

void CollisionHandler::EndContact(b2Contact* pContact)
{
	for (auto* pContact : m_pContacts)
	{
		b2Fixture* pA = pContact->GetFixtureA();
		b2Fixture* pB = pContact->GetFixtureB();
	
		GameObject * pAObject = static_cast<GameObject*>(pA->GetBody()->GetUserData());
		GameObject* pBObject = static_cast<GameObject*>(pB->GetBody()->GetUserData());
	
		if (pAObject && pBObject)
		{
			pAObject->OnContactEnd(pB);
			pBObject->OnContactEnd(pA);
		}
		
	}

	m_pContacts.clear();

}

void CollisionHandler::HandleCollisions()
{
	for (auto* pContact : m_pContacts)
	{
		b2Fixture* pA = pContact->GetFixtureA();
		b2Fixture* pB = pContact->GetFixtureB();

		GameObject * pAObject = static_cast<GameObject*>(pA->GetBody()->GetUserData());
		GameObject* pBObject = static_cast<GameObject*>(pB->GetBody()->GetUserData());

		if (pAObject && pBObject)
		{
			if (pA->IsSensor() || pB->IsSensor())
			{
				pAObject->OnTriggerEnter(pB);
				pBObject->OnTriggerEnter(pA);
			}
			else
			{
				pAObject->OnCollisionEnter(pB);
				pBObject->OnCollisionEnter(pA);
				
			}
		}
		
	}


}

void CollisionHandler::ClearContacts()
{
	m_pContacts.clear();
}
