import { useState } from "react";
import { Button } from "./Button";

export const Counter = () => {
    const [count, setCount] = useState<number>(0);

    const increase = () => {
        if (count < 5) setCount(count + 1);
    };

    const reset = () => {
        setCount(0);
    };

    return (
        <div  className="counter_block">
            <h1>{count}</h1>

            <div className="interaction_buttons">
                <Button
                    title="inc"
                    callback={increase}
                    disabled={count === 5}
                />

                <Button
                    title="reset"
                    callback={reset}
                    disabled={count === 0}
                />
            </div>
        </div>
    );
};