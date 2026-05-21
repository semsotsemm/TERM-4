interface FormTitleProps{
  title: string;
  hint:string;
}

const FormTitle = ({title, hint}: FormTitleProps) => {
  return (
    <>      
      <h2 className="auth-title">{title}</h2>
      <p className="auth-subtitle">{hint}</p>
    </>
  )
}

export default FormTitle