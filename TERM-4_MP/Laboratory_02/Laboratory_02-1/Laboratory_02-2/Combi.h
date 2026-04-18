#pragma once 

namespace combi
{
    struct  Combination            
    {
        short  n, m, *sequence;

        Combination(short n = 1, short m = 1);

        void reset();              // сбросить генератор, начать сначала 
        short getfirst();          // сформировать первый массив индексов    
        void getNextCombination();           // сформировать следующий массив индексов  
        unsigned int count_combinations();
    };
}