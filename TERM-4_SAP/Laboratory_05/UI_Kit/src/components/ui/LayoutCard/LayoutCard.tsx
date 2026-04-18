import React, {type ReactNode } from 'react';
import styles from './LayoutCard.module.css';

export interface LayoutCardProps {
  title: ReactNode;
  children: ReactNode;
  footer?: ReactNode;
}

export const LayoutCard: React.FC<LayoutCardProps> = ({ title, children, footer }) => {
  return (
    <div className={styles.card}>
      <div className={styles.header}>
        {title}
      </div>
      <div className={styles.body}>
        {children}
      </div>
      {footer && (
        <div className={styles.footer}>
          {footer}
        </div>
      )}
    </div>
  );
};