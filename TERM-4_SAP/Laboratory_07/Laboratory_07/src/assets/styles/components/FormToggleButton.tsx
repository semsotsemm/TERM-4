interface form_toggle_button{
    button_text: string;
    is_active: boolean;
}

const FormToggleButton = ({button_text, is_active}: form_toggle_button) => {
    return (
         <button className={is_active? "tab active": "tab"}>{button_text}</button>
    )
}

export default FormToggleButton