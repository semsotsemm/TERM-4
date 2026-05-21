interface text_with_accent{
    text: string;
    accent: string;
}

const TextWithAccent = ({text, accent}: text_with_accent) => {
    return(
    <h1>{text} <em>{accent}</em></h1>
    )
}

export default TextWithAccent