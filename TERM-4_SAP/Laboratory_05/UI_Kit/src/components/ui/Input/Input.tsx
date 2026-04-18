  import React, { type InputHTMLAttributes } from 'react';
  import classNames from 'classnames';
  import styles from './Input.module.css';

  export interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
    label: string;
    error?: string;
    isFullWidth?: boolean;
  }

  export const Input: React.FC<InputProps> = ({
    label,
    error,
    isFullWidth = false,
    className,
    ...props
  }) => {
    const wrapperClass = classNames(styles.wrapper, {
      [styles.fullWidth]: isFullWidth,
    });

    const inputClass = classNames(
      styles.input,
      { [styles.hasError]: !!error },
      className
    );

    return (
      <div className={wrapperClass}>
        <label className={styles.label}>{label}</label>
        <input className={inputClass} {...props} />
        {error && <span className={styles.errorMessage}>{error}</span>}
      </div>
    );
  };