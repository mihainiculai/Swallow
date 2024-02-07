const DividerWithText = ({ text }: { text: string }) => (
    <div className="flex items-center justify-center my-8">
        <div className="flex-grow border-t"></div>
        <span className="mx-4">{text}</span>
        <div className="flex-grow border-t"></div>
    </div>
);

export default DividerWithText;