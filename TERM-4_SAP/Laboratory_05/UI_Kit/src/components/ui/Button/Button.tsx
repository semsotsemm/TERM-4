import React, { 
  type ButtonHTMLAttributes, 
  type PropsWithChildren 
} from 'react';
import classNames from 'classnames';
import styles from './Button.module.css';

export interface ButtonProps extends PropsWithChildren<ButtonHTMLAttributes<HTMLButtonElement>> {
  variant?: 'primary' | 'secondary' | 'danger';
  size?: 'small' | 'medium' | 'large';
  isLoading?: boolean;
  isFullWidth?: boolean; 
}

export const Button: React.FC<ButtonProps> = ({
  variant = 'primary',
  size = 'medium',
  isLoading = false,
  isFullWidth = false,
  children,
  className,
  disabled,
  ...props
}) => {
  const btnClass = classNames(
    styles.button,
    styles[variant],
    styles[size],
    { 
      [styles.loading]: isLoading,
      [styles.fullWidth]: isFullWidth 
    },
    className
  );

  return (
    <button
      className={btnClass}
      disabled={isLoading || disabled}
      {...props}
    >
      {isLoading ? 'Загрузка...' : children}
    </button>
  );
};