import React from 'react';
import classNames from 'classnames';
import styles from './Badge.module.css';

export interface BadgeProps {
  color: 'green' | 'red' | 'orange' | 'blue';
  text: string;
}

export const Badge: React.FC<BadgeProps> = ({ color, text }) => {
  const badgeClass = classNames(styles.badge, styles[color]);

  return <span className={badgeClass}>{text}</span>;
};