// src/components/Button.tsx
import React from 'react';

interface ButtonProps {
  onClick?: () => void;
  className?: string;
  disabled?: boolean;
  color?: 'green' | 'red' | 'blue' | 'yellow'; // Add other colors as needed
  type?: 'button' | 'submit' | 'reset';
  children: React.ReactNode;
}

const Button: React.FC<ButtonProps> = ({ onClick, className, disabled, color = 'green', type = 'button', children }) => {
  const baseClasses = `text-white p-2 w-full max-w-xs rounded-md focus:outline-none focus:ring-2 focus:ring-offset-2`;
  let colorClasses = '';

  switch (color) {
    case 'red':
      colorClasses = 'bg-red-600 hover:bg-red-700 focus:ring-red-500';
      break;
    case 'blue':
      colorClasses = 'bg-blue-600 hover:bg-blue-700 focus:ring-blue-500';
      break;
    case 'yellow':
      colorClasses = 'bg-yellow-600 hover:bg-yellow-700 focus:ring-yellow-500';
      break;
    case 'green':
    default:
      colorClasses = 'bg-green-600 hover:bg-green-700 focus:ring-green-500';
      break;
  }

  return (
    <div className="flex justify-center">
      <button
        type={type}
        onClick={onClick}
        className={`${baseClasses} ${colorClasses} ${className}`}
        disabled={disabled}
      >
        {children}
      </button>
    </div>
  );
};

export default Button;