interface RequiredProps {
    required?: boolean;
}

export const Required: React.FC<RequiredProps> = ({ required = true }) => {
    return (
        <span className="text-danger mx-1">{required ? "*" : ""}</span>
    );
}