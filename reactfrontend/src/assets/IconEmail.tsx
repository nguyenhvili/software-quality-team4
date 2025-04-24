export function IconEmail(props: { className: string }) {
    return (
        <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth={1.5}
            stroke="currentColor"
            className={`size-6 ${props.className ?? ""}`}
        >
            <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M3.75 21l16.5-9L3.75 3v6.75l11.25 2.25-11.25 2.25V21z"
            />
        </svg>
    );
}
