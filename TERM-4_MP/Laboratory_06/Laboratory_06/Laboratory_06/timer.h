#pragma once

#include "stdafx.h"

class Timer 
{
	Timer() 
	{
		minutes = 0;
		seconds = 0;
		start_counting = 0;
		time(&current_time);
	}

	public: unsigned short minutes;
	public: unsigned short seconds;

	private: time_t current_time;
	private: unsigned int start_counting;

	void TimerStart();
	void TimerStop();
	void GetTime();
};