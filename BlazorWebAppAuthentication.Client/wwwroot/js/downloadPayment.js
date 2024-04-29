function downloadPayment(filename, contentType, content) {
    const link = document.createElement('a');
    link.href = `data:${contentType};base64,${content}`;
    link.download = filename;
    link.click();
}