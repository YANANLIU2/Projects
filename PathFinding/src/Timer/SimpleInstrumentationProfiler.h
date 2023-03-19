
#pragma once

// Only define the class if we're not in the profile build configuration.  If we are in the profile build 
// configuration, it's assumed that the Visual Studio profiler is being used.
#ifndef _PROFILE

#include "HighPrecisionTimer.h"
#include <string>
#include <iostream>
#include <assert.h>

class SimpleInstrumentationProfiler
{
	HighPrecisionTimer m_timer;
	std::string m_label;

public:
	SimpleInstrumentationProfiler(const char* label)
		: m_label(label)
	{
		m_timer.StartTimer();
	}

	~SimpleInstrumentationProfiler()
	{
		double result = m_timer.GetTimer();
		std::cout << "Result for " << m_label << ": " << result << std::endl;
	}
};

#define START_PROFILER(_str_) SimpleInstrumentationProfiler profiler(_str_)

#else  // _PROFILE, so we need to disable this timer

#define START_PROFILER(_str_)

#endif  // _PROFILE
