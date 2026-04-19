#include "timer.h"

using namespace std;

void Timer::TimerStart()
{
	time(&current_time);
	start_counting = 0;
	minutes = 0;
	seconds = 0;
}

void Timer::TimerStop()
{
	start_counting = current_time;
	time(&current_time);
	minutes = short((current_time - start_counting) / 60);
	seconds = (current_time - start_counting) % 60;
}

void Timer::GetTime()
{
	cout << "Прошедшее время: " << minutes << "м. " << seconds << "с.\n";
}