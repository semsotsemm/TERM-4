#pragma once

namespace combi
{
    struct Transposition
    {
        struct Element
        {
            short element;
            bool arrow;
            Element(short el, bool arr);
        };

        Transposition(short size);

        short sequence_size;
        Element* sequence;

        int getNextTransp();
        int findMaxMobile();
    };
}